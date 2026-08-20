#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
//
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
//
// The Original Code is MiNET.
//
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
//
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2026 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using MiNET.Utils.Cryptography;
using Newtonsoft.Json;

namespace MiNET.Net.NetherNet
{
	/// <summary>
	///     Builds the <c>a=identity</c> attribute a NetherNet peer puts in its SDP, which is how an
	///     authenticated player proves the DTLS certificate in the offer is theirs.
	///     <para>
	///         The chain is: the auth service signs a token naming a public key (<c>cpk</c>), the
	///         private half of that key signs the SDP's fingerprint lines, and WebRTC guarantees the
	///         DTLS certificate matches those fingerprints. Altering a fingerprint invalidates the
	///         signature, and replaying a signed one is useless without the DTLS private key.
	///     </para>
	///     <para>See <c>docs/bedrock-authentication.md</c>.</para>
	/// </summary>
	public static class NetherNetIdentityAssertion
	{
		/// <summary>
		///     Matches an SDP fingerprint line at any level, since implementations differ over whether
		///     it sits in the session or the media section, and the signature covers all of them.
		/// </summary>
		private static readonly Regex FingerprintLine = new(@"^a=fingerprint:(?<algorithm>\S+)\s+(?<digest>\S+)\s*$",
			RegexOptions.Multiline | RegexOptions.Compiled);

		/// <summary>
		///     Returns the offer with an <c>a=identity</c> attribute inserted, or the offer unchanged
		///     when no identity is supplied, which is the offline case a permissive server may accept.
		/// </summary>
		public static string AddTo(string sdp, XboxIdentity identity, string issuerDomain)
		{
			// 26.50 BDS rejects an offer with no a=identity at all (numeric error body instead of
			// an answer SDP), so the offline case sends a "self" assertion: an ephemeral key that
			// vouches for itself, the same shape BDS uses in its own answers.
			if (identity == null) return AddSelfTo(sdp);

			string assertion = Build(sdp, identity, issuerDomain);

			// Session level, so it goes before the first media section. Everything after that first
			// "m=" belongs to a media description and an attribute placed there is media level.
			int firstMedia = sdp.IndexOf("m=", StringComparison.Ordinal);
			string line = $"a=identity:{assertion}\r\n";

			return firstMedia < 0 ? sdp + line : sdp.Insert(firstMedia, line);
		}

		/// <summary>The base64 envelope carrying the token and the detached signature over the fingerprints.</summary>
		public static string Build(string sdp, XboxIdentity identity, string issuerDomain)
		{
			string canonicalFingerprints = CanonicalFingerprints(sdp);
			string detachedJws = SignDetached(canonicalFingerprints, identity.IdentityKey);

			// The inner assertion is a JSON string containing JSON, not a nested object.
			string inner = JsonConvert.SerializeObject(new
			{
				token = identity.LoginToken,
				fingerprints = detachedJws
			});

			string envelope = JsonConvert.SerializeObject(new
			{
				idp = new {domain = issuerDomain, protocol = "default"},
				assertion = inner
			});

			return Convert.ToBase64String(Encoding.UTF8.GetBytes(envelope));
		}

		/// <summary>
		///     Serialises every fingerprint in the SDP the one way both sides must agree on. The
		///     payload is omitted from the JWS, so the verifier rebuilds these exact bytes from the
		///     SDP: any difference in key order or spacing breaks the signature rather than producing
		///     a readable error.
		/// </summary>
		public static string CanonicalFingerprints(string sdp)
		{
			var fingerprints = new List<string>();

			foreach (Match match in FingerprintLine.Matches(sdp))
			{
				// Keys sorted lexicographically: algorithm before digest. No whitespace anywhere.
				string algorithm = JsonEscape(match.Groups["algorithm"].Value);
				string digest = JsonEscape(match.Groups["digest"].Value);

				fingerprints.Add($"{{\"algorithm\":\"{algorithm}\",\"digest\":\"{digest}\"}}");
			}

			if (fingerprints.Count == 0) throw new InvalidOperationException("The SDP carries no a=fingerprint line to sign");

			return $"{{\"fingerprint\":[{string.Join(",", fingerprints)}]}}";
		}

		/// <summary>
		///     The offline client identity: an ephemeral P-384 key self-signs a 60-second token
		///     carrying its own public key (cpk as a JWK object, x5u header with the DER SPKI), and
		///     the same key signs the fingerprints. Mirrors byte-for-byte the shape BDS 1.26.50.26
		///     puts in its own answers with idp domain "self" (relay capture, 2026-08-19).
		/// </summary>
		public static string AddSelfTo(string sdp)
		{
			using ECDsa key = ECDsa.Create(ECCurve.NamedCurves.nistP384);
			ECParameters parameters = key.ExportParameters(false);
			long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

			string token = Jose.JWT.Encode(new Dictionary<string, object>
			{
				["cpk"] = new Dictionary<string, object>
				{
					["crv"] = "P-384",
					["kty"] = "EC",
					["x"] = Base64Url(parameters.Q.X),
					["y"] = Base64Url(parameters.Q.Y)
				},
				["exp"] = now + 60,
				["iat"] = now
			}, key, Jose.JwsAlgorithm.ES384, extraHeaders: new Dictionary<string, object>
			{
				["x5u"] = Convert.ToBase64String(key.ExportSubjectPublicKeyInfo())
			});

			string inner = JsonConvert.SerializeObject(new
			{
				fingerprints = SignDetached(CanonicalFingerprints(sdp), key),
				token
			});

			string envelope = JsonConvert.SerializeObject(new
			{
				assertion = inner,
				idp = new {domain = "self", protocol = "default"}
			});

			string line = $"a=identity:{Convert.ToBase64String(Encoding.UTF8.GetBytes(envelope))}\r\n";

			int firstMedia = sdp.IndexOf("m=", StringComparison.Ordinal);
			return firstMedia < 0 ? sdp + line : sdp.Insert(firstMedia, line);
		}

		/// <summary>
		///     Builds the server's side of the same mechanism, for the SDP answer. The token here is
		///     self-signed with a long-lived operator key rather than issued by Mojang: the client
		///     pins that key the first time it sees it and accepts silently afterwards, so trust
		///     attaches to the key and not to our address. Rotating it re-prompts every player.
		/// </summary>
		public static string AddServerAssertionTo(string answerSdp, ECDsa operatorKey, string domain, string issuer)
		{
			string cpk = XboxAuthentication.PublicKeyBase64(operatorKey);
			long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

			ECDsa signKey = operatorKey;

			// A plain self-signed JWT. Nothing validates it centrally; the client verifies the
			// signature with the cpk carried inside it, which is the value it pins.
			string token = Jose.JWT.Encode(new Dictionary<string, object>
			{
				["cpk"] = cpk,
				["iss"] = issuer,
				["iat"] = now,
				["exp"] = now + (long) TimeSpan.FromDays(365).TotalSeconds
			}, signKey, Jose.JwsAlgorithm.ES384);

			string inner = JsonConvert.SerializeObject(new
			{
				token,
				fingerprints = SignDetached(CanonicalFingerprints(answerSdp), operatorKey)
			});

			string envelope = JsonConvert.SerializeObject(new
			{
				idp = new {domain, protocol = "default"},
				assertion = inner
			});

			string line = $"a=identity:{Convert.ToBase64String(Encoding.UTF8.GetBytes(envelope))}\r\n";

			int firstMedia = answerSdp.IndexOf("m=", StringComparison.Ordinal);
			return firstMedia < 0 ? answerSdp + line : answerSdp.Insert(firstMedia, line);
		}

		/// <summary>
		///     RFC 7515 Appendix F: compact serialization with the payload left out, so the result is
		///     header..signature rather than header.payload.signature.
		/// </summary>
		private static string SignDetached(string payload, ECDsa key)
		{
			// ES384 pairs with the P-384 identity key the auth service named in cpk.
			string header = Base64Url(Encoding.UTF8.GetBytes("{\"alg\":\"ES384\"}"));
			string encodedPayload = Base64Url(Encoding.UTF8.GetBytes(payload));

			byte[] signingInput = Encoding.ASCII.GetBytes($"{header}.{encodedPayload}");

			ECDsa signer = key;
			// IeeeP1363 is the fixed-width r||s form JOSE requires, not the ASN.1 DER default.
			byte[] signature = signer.SignData(signingInput, HashAlgorithmName.SHA384, DSASignatureFormat.IeeeP1363FixedFieldConcatenation);

			return $"{header}..{Base64Url(signature)}";
		}

		private static string JsonEscape(string value) => value.Replace("\\", "\\\\").Replace("\"", "\\\"");

		private static string Base64Url(byte[] data) =>
			Convert.ToBase64String(data).Replace('+', '-').Replace('/', '_').TrimEnd('=');
	}
}
