using System;
using System.Threading;
using log4net;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using MiNET.Worlds;
using MiNET.InPVP.Annotations;
using System.Configuration;

namespace MiNET.InPVP
{
    [Plugin(PluginName = "MiNet InPVP", Description = "A plugin made to show how cool I am.", PluginVersion = "1.0", Author = "Elfocrash"), UsedImplicitly]
    public class InPVPPlugin : Plugin
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(InPVPPlugin));

        #region configuration
        static int _popupMessageInterval = Int32.Parse(ConfigurationManager.AppSettings["PopupMessageInverval"]);
        static int _chatMessageInterval = Int32.Parse(ConfigurationManager.AppSettings["ChatMessageInverval"]);
        static string _popupMessageText = ConfigurationManager.AppSettings["PopupMessage"];
        static string _chatMessageText = ConfigurationManager.AppSettings["PopupMessage"];
        static int _nonChangableBlockBySpawnDistance = Int32.Parse(ConfigurationManager.AppSettings["DistanceFromSpawnInBlocks"]);
        #endregion

        [UsedImplicitly]
        private Timer _autoMessagePopupTimer;

        [UsedImplicitly]
        private Timer _autoMessageChatTimer;

        protected override void OnEnable()
        {
            var server = Context.Server;
            RegisterServerEvents(server);
            //Note here that the first int represents when the popup will start broadcasting in milliseconds and then the interval in milliseconds
            _autoMessagePopupTimer = new Timer(BroadcastPopupMessage, null, _popupMessageInterval, _popupMessageInterval);
            _autoMessageChatTimer = new Timer(BroadcastChatMessage, null, _chatMessageInterval, _chatMessageInterval);
        }

        private void RegisterServerEvents(MiNetServer server)
        {
            server.LevelManager.LevelCreated += (sender, args) =>
            {
                Level level = args.Level;
                level.BlockBreak += LevelOnBlockBreak;
                level.BlockPlace += LevelOnBlockPlace;
            };

            server.PlayerFactory.PlayerCreated += (sender, args) =>
            {
                Player player = args.Player;
                player.PlayerJoin += OnPlayerJoin;
                player.PlayerLeave += OnPlayerLeave;
            };
        }

        private void OnPlayerLeave(object o, PlayerEventArgs eventArgs)
        {
            HandlePlayerLogout(eventArgs);
        }

        private void OnPlayerJoin(object o, PlayerEventArgs eventArgs)
        {
            Level level = HandlePlayerJoinedLevel(eventArgs);
        }

        private void LevelOnBlockBreak(object sender, BlockBreakEventArgs e)
        {
            DisableBlockBreakingFromSpawnPointByBlock(e, _nonChangableBlockBySpawnDistance);
        }

        private void LevelOnBlockPlace(object sender, BlockPlaceEventArgs e)
        {
            DisableBlockPlacingFromSpawnPointByBlock(e, _nonChangableBlockBySpawnDistance);
        }

        #region helper methods
            private static void DisableBlockPlacingFromSpawnPointByBlock(BlockPlaceEventArgs e, int distance)
            {
                if (e.ExistingBlock.Coordinates.DistanceTo((BlockCoordinates)e.Player.SpawnPosition) < distance)
                {
                    e.Cancel = e.Player.GameMode != GameMode.Creative;
                }
            }

            private static void DisableBlockBreakingFromSpawnPointByBlock(BlockBreakEventArgs e, int distance)
            {
                if (e.Block.Coordinates.DistanceTo((BlockCoordinates)e.Player.SpawnPosition) < distance)
                {
                    e.Cancel = e.Player.GameMode != GameMode.Creative;
                }
            }

            private static Level HandlePlayerJoinedLevel(PlayerEventArgs eventArgs)
            {
                Level level = eventArgs.Level;
                if (level == null) throw new ArgumentNullException(nameof(eventArgs.Level));

                Player player = eventArgs.Player;
                if (player == null) throw new ArgumentNullException(nameof(eventArgs.Player));
                return level;
            }

            private static void HandlePlayerLogout(PlayerEventArgs eventArgs)
            {
                Level level = eventArgs.Level;
                if (level == null) throw new ArgumentNullException(nameof(eventArgs.Level));

                Player player = eventArgs.Player;
                if (player == null) throw new ArgumentNullException(nameof(eventArgs.Player));
            }

            private void BroadcastPopupMessage(object state)
            {
                foreach (var level in Context.LevelManager.Levels)
                {
                    var players = level.GetSpawnedPlayers();
                    foreach (var player in players)
                    {
                        player.AddPopup(AutomatedPopup);
                    }
                }
            }

            private void BroadcastChatMessage(object state)
            {
                foreach (var level in Context.LevelManager.Levels)
                {
                    level.BroadcastMessage(_chatMessageText);
                }
            }

            private Popup AutomatedPopup = new Popup()
            {
                MessageType = MessageType.Tip,
                Message = _popupMessageText,
                Duration = 20 * 10
            };
            #endregion
    }
}