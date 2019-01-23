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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System.Collections.Generic;
using MiNET;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.UI;
using Stripe;
using Input = MiNET.UI.Input;

namespace TestPlugin.SafePay
{
	[Plugin(PluginName = "SafePay", Description = "Safe payment solution for MiNET", PluginVersion = "1.0", Author = "MiNET Team")]
	public class SafePayPlugin : Plugin
	{
		protected override void OnEnable()
		{
		}

		[Command]
		public void BuyCoins(Player player)
		{
			var simpleForm = new SimpleForm();
			simpleForm.Title = "Store - Buy coins";
			simpleForm.Content = "Select the amount of coins you want to buy.";
			simpleForm.Buttons = new List<Button>()
			{
				new Button
				{
					Text = " 320 Coins -  $1.90",
					Image = new Image
					{
						Type = "url",
						Url = "https://i.imgur.com/SedU2Ad.png"
					},
					ExecuteAction = ExecuteSelectPaymentMethod
				},
				new Button
				{
					Text = "1032 Coins -  $4.95",
					Image = new Image
					{
						Type = "url",
						Url = "https://i.imgur.com/oBMg5H3.png"
					},
					ExecuteAction = ExecuteSelectPaymentMethod
				},
				new Button
				{
					Text = "3021 Coins - $10.05",
					Image = new Image
					{
						Type = "url",
						Url = "https://i.imgur.com/hMAfqQd.png"
					},
					ExecuteAction = ExecuteSelectPaymentMethod
				},
				new Button {Text = "Cancel"},
			};

			player.SendForm(simpleForm);
		}

		private void ExecuteSelectPaymentMethod(Player player, SimpleForm form)
		{
			var simpleForm = new SimpleForm();
			simpleForm.Title = "Checkout - Select payment method";
			simpleForm.Content = "Select the payment method you like to use for your purchase.";
			simpleForm.Buttons = new List<Button>()
			{
				new Button
				{
					Text = "Pay with VISA",
					Image = new Image
					{
						Type = "url",
						Url = "http://www.credit-card-logos.com/images/visa_credit-card-logos/visa_logo_6.gif"
					},
					ExecuteAction = ExecutePayVisa
				},
				new Button
				{
					Text = "Pay with Mastercard",
					Image = new Image
					{
						Type = "url",
						Url = "http://www.credit-card-logos.com/images/mastercard_credit-card-logos/mastercard_logo_6.gif"
					}
				},
				new Button
				{
					Text = "Use PayPal",
					Image = new Image
					{
						Type = "url",
						Url = "http://logok.org/wp-content/uploads/2014/05/Paypal-logo-20141.png"
					}
				},
				new Button {Text = "Cancel"},
			};

			player.SendForm(simpleForm);
		}

		private void ExecutePayVisa(Player player, SimpleForm form)
		{
			CustomForm customForm = new CustomForm();
			customForm.Title = "Secure Payment Info - VISA";
			customForm.ExecuteAction = ExecuteReviewOrder;
			customForm.Content = new List<CustomElement>()
			{
				new Label {Text = "Safe money transfer using your VISA card"},
				new Input
				{
					Text = "",
					Placeholder = "Name - as it appears on card",
					Value = "John Doe"
				},
				new Input
				{
					Text = "",
					Placeholder = "Credit card number",
					Value = "4242424242424242"
				},
				new Dropdown
				{
					Text = "Expiration date (month)",
					Options = new List<string>()
					{
						"01 - January",
						"02 - February",
						"03 - March"
					},
					Value = 0
				},
				new Dropdown
				{
					Text = "Expiration date (year)",
					Options = new List<string>()
					{
						"2017",
						"2018",
						"2019"
					},
					Value = 2
				},
				new Input
				{
					Text = "",
					Placeholder = "Security code (3 on back)",
					Value = "111"
				},
				new Toggle
				{
					Text = "Save payment info (safe)",
					Value = true
				},
				new Label {Text = "§lWhat happens now?§r\nThis is step 1 of 2. After submitting payment information you will be able to review your order.\nWe will not bill you until confirm the order on next page (step 2)."},
			};

			player.SendForm(customForm);
		}

		private void ExecuteReviewOrder(Player player, CustomForm form)
		{
			var modalForm = new ModalForm();
			modalForm.ExecuteAction = ExecutePayment;
			modalForm.Title = "Review Order";
			modalForm.Content = "§lPlease review your ordering information below.§r\nProduct: Mega coins extra pack.\nYour total: $3.99 USD\nPayment method: VISA ************59 $3.99 USD\n";
			modalForm.Button1 = "§2§lBuy now";
			modalForm.Button2 = "Cancel";

			player.SendForm(modalForm);
		}

		private void ExecutePayment(Player player, ModalForm modal)
		{
			StripeConfiguration.SetApiKey("sk_test_PnZiNirxxNCSVfnAvdADv85L"); // Add your API key here

			double amount = 1.90;

			var chargeOptions = new StripeChargeCreateOptions()
			{
				Amount = (int?) (amount * 100),
				Currency = "usd",
				Description = "Charge for rich.dude@mailinator.com",
				SourceCard = new SourceCard()
				{
					Number = "4242424242424242",
					ExpirationMonth = 01,
					ExpirationYear = 2025,
					Cvc = "111",
					//Capture = true
				},
			};

			var chargeService = new StripeChargeService();
			StripeCharge charge = chargeService.Create(chargeOptions);
		}
	}
}