using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;
using MiNET;
using MiNET.Plugins.Attributes;
using MiNET.Plugins;
using MiNET.Worlds;
using MiNET.Utils;
using MiNET.Blocks;
using System.Threading;
using MiNET.Security;
using MiNET.Net;

namespace CorePlugins
{

    public struct User
    {
        public string nick;
        public ModelUser model;
        public Player player;
        public string ip;

        public User Copy(string nick = null, ModelUser model = new ModelUser { id = -1 }, Player player = null, string ip = "")
        {
            return new User { nick = nick ?? this.nick, ip = ip ?? this.ip, model = model.id == -1 ? this.model : model, player = player ?? this.player };
        }
    }

    [Plugin(PluginName = "CoreCommands", Description = "The core.", PluginVersion = "1.0", Author = "Artem Valko")]
    public class CoreCommands : Plugin
    {
        private List<User> users = new List<User>();
        private Dictionary<string, string> preLoginUser = new Dictionary<string, string>();
        private DataBase db;
        private PlayerLocation spawnPosition;

        protected override void OnEnable()
        {
            foreach (var level in Context.LevelManager.Levels)
            {
                level.BlockBreak += LevelOnBlockBreak;
                level.BlockPlace += LevelOnBlockPlace;
            }

            _timerOnBlock = new Timer(TimerOnBlock, null, 30000, 5000);

            db = new DataBase();

            Context.PluginManager.HandleCommandAction += HandleCommandAction;
            Player.HandleStartAction += HandleStartAction;
            Player.HandleDisconnectAction += HandleDisconnectAction;

            var tmp = McpeSetSpawnPosition.CreateObject();
            spawnPosition = new PlayerLocation(tmp.x, tmp.y, tmp.z);
        }

        private void HandleStartAction(object sender, EventArgs e)
        {
            // Добавление нового игрока
            Player target = sender as Player;
            ModelUser model;

            if(users.Exists(x => x.nick == target.Username) || preLoginUser.ContainsKey(target.Username))
            {
                Disconect("Double login.", target);
                return;
            }

            // зареган?
            if(db.HasUser(target.Username, out model))
            {
                ProcessLogin(target, model);
            }
            else
            {
                target.isRegister = false;
                ProcessRegister(target);
            }


        }

        private void HandleDisconnectAction(object sender, EventArgs e)
        {
            // Отключение игрока
            Player target = sender as Player;
            target.isLogin = true; // :)
            target.isRegister = true; // :)
            users.RemoveAll(x => x.nick == target.Username);
            preLoginUser.Remove(target.Username);
        }

        private void HandleCommandAction(object sender, PluginManager.HandleCommandActionEventArgs e)
        {
            string nick = e.player.Username;
            int i = 0;
            if((i = users.FindLastIndex(x => x.nick == nick)) != -1)
            {
                ModelUser model;
                db.HasUser(nick, out model);
                users[i] = users[i].Copy(model: model);
                e.Cancel = model.role < e.RoleRequired;
                return;
            }
            bool auth = e.command == "l" || e.command == "r" || e.command == "register" || e.command == "login";
            e.Cancel = !(auth && !e.player.isLogin);
        }

        private void Disconect(string message, Player player)
        {
            player.Disconnect(message, true, false);
        }

        #region Block Change
        static readonly object _lockOnBlockAdd = new object();
        private List<Block> _onBlockCoordinates = new List<Block>();
        private Timer _timerOnBlock;

        private void LevelOnBlockBreak(object sender, BlockBreakEventArgs e)
        {             
            if(!e.Player.isLogin)
            {
                e.Cancel = true;
                return;
            }

            lock (_lockOnBlockAdd)
            {
                //Console.WriteLine("break lock");
                Block temp = new Block(0) { Coordinates = e.Block.Coordinates };

                int i = _onBlockCoordinates.FindLastIndex((x) => x.Coordinates.X == temp.Coordinates.X && x.Coordinates.Z == temp.Coordinates.Z && x.Coordinates.Y == temp.Coordinates.Y);
                if (i == -1)
                    _onBlockCoordinates.Add(temp);
                else
                {
                    _onBlockCoordinates[i] = temp;
                }
                Console.WriteLine("break unlocked block = {0}; x={1};y={2};z={3}", temp.Id, temp.Coordinates.X, temp.Coordinates.Y, temp.Coordinates.Z);
            }
        }

        private void LevelOnBlockPlace(object sender, BlockPlaceEventArgs e)
        {
            if (!e.Player.isLogin)
            {
                e.Cancel = true;
                return;
            }

            lock (_lockOnBlockAdd)
            {
                Block temp = new Block((byte)e.Next.Id) { Coordinates = e.Next.GetNewCoordinatesFromFace(e.Block.Coordinates, e.Face) };
                //Console.WriteLine("place lock");
                int i = _onBlockCoordinates.FindLastIndex((x) => x.Coordinates.X == temp.Coordinates.X && x.Coordinates.Z == temp.Coordinates.Z && x.Coordinates.Y == temp.Coordinates.Y);
                if (i == -1)
                    _onBlockCoordinates.Add(temp);
                else
                {
                    _onBlockCoordinates[i] = temp;
                }

                Console.WriteLine("place unlocked block = {0}; x={1};y={2};z={3}", temp.Id, temp.Coordinates.X, temp.Coordinates.Y, temp.Coordinates.Z);
            }
        }

        private void TimerOnBlock(object state)
        {
            //_timerOnBlock.Change(Timeout.Infinite, Timeout.Infinite);
            List<Block> copy;
            lock (_lockOnBlockAdd)
            {
                copy = new List<Block>(_onBlockCoordinates);
                _onBlockCoordinates.Clear();
            }

            foreach (Block block in copy)
            {
                int x = block.Coordinates.X, z = block.Coordinates.Z, y = block.Coordinates.Y;
                var anvil = new AnvilWorldProvider("world");
                anvil.Initialize();
                ChunkCoordinates coordinates = new ChunkCoordinates(x >> 4, z >> 4);
                ChunkColumn chunk = anvil.GenerateChunkColumn(coordinates);
                chunk.SetBlock(x % 16, y, z % 16, block.Id);
                AnvilWorldProvider.SaveChunk(chunk, "world", 0);
            }

            //Console.WriteLine("Save... {0}", copy.Count);
        }


       /* private struct _tempItemOnBlock
        {
            public ChunkColumn column;
            public List<Block> list;
        }
        
        private void TimerOnBlock(object state)
        {
           // _timerOnBlock.Change(Timeout.Infinite, Timeout.Infinite);
            List<Block> copy;
            lock (_lockOnBlockAdd)
            {
                copy = new List<Block>(_onBlockCoordinates);
                _onBlockCoordinates.Clear();
            }
            //Console.WriteLine("save...");
            List<_tempItemOnBlock> dict = new List<_tempItemOnBlock>();



            foreach(Block block in copy)
            {
                int x = block.Coordinates.X, z = block.Coordinates.Z, y = block.Coordinates.Y;
                var anvil = new AnvilWorldProvider("world");
                anvil.Initialize();
                ChunkCoordinates coordinates = new ChunkCoordinates(x >> 4, z >> 4);
                ChunkColumn chunk = anvil.GenerateChunkColumn(coordinates);

                int i = dict.FindLastIndex(item => item.column.z == chunk.z && item.column.x == chunk.x);
                if (i == -1)
                {
                    dict.Add(new _tempItemOnBlock { column = chunk, list = new List<Block> { block } });
                }
                else
                {
                    dict[i].list.Add(block);
                }
            }

            foreach (var item in dict)
            {
                foreach (var block in item.list)
                {
                    int x = block.Coordinates.X, z = block.Coordinates.Z, y = block.Coordinates.Y;
                    item.column.SetBlock(x % 16, y, z % 16, block.Id);
                }
                AnvilWorldProvider.SaveChunk(item.column, "world", 0);
            }

            Console.WriteLine("save {0} (1: {1})", dict.Count, dict[1].list.Count);
        }*/
        
        #endregion

        #region Auth
        [Command(Command = "login")]
        public void commandLogin(Player player, string pass)
        {
            LoginCommand(player, pass);
        }

        [Command(Command = "l")]
        public void commandLittleLogin(Player player, string pass)
        {
            LoginCommand(player, pass);
        }

        [Command(Command = "login")]
        public void commandLogin(Player player)
        {
            player.SendMessage("[Auth] Использование: /login password");
        }

        [Command(Command = "l")]
        public void commandLittleLogin(Player player)
        {
            player.SendMessage("[Auth] Использование: /l password");
        }


        [Command(Command = "register")]
        public void commandRegister(Player player, string pass)
        {
            RegisterCommand(player, pass);
        }

        [Command(Command = "r")]
        public void commandLittleRegister(Player player, string pass)
        {
            RegisterCommand(player, pass);
        }

        [Command(Command = "register")]
        public void commandRegister(Player player)
        {
            player.SendMessage("[Auth] Использование: /register password");
        }

        [Command(Command = "r")]
        public void commandLittleRegister(Player player)
        {
            player.SendMessage("[Auth] Использование: /r password");
        }

        public void LoginCommand(Player player, string pass)
        {
            if (player.isLogin || !player.isRegister)
                return;

            if (preLoginUser[player.Username] == pass)
            {
                preLoginUser.Remove(player.Username);
                ModelUser model;
                db.HasUser(player.Username, out model);
                users.Add(new User { ip = player.EndPoint.Address.ToString(), model = model, nick = player.Username, player = player });
                player.isLogin = true;
                player.ClearPopups();
                player.AddPopup(new Popup()
                {
                    MessageType = MessageType.Tip,
                    Message = "§2Авторизация успешна!",
                    Duration = 20 * 4
                });

                player.SendMessage("[Auth] Вы вошли!");
            }
            else
            {
                player.SendMessage("[Auth] Не правильный пароль!");
            }
        }

        public void RegisterCommand(Player player, string pass)
        {
            if (player.isLogin || player.isRegister)
                return;

            if(pass.Length > 30)
            {
                player.SendMessage("[Auth] Пароль слишком длинный!");
                return;
            }
            else if(pass.Length < 6)
            {
                player.SendMessage("[Auth] Пароль слишком короткий! (Не менее 6 символов)");
                return;
            }

            db.AddUser(player.Username, pass);
            player.isLogin = true;

            player.ClearPopups();

            player.SendMessage("[Auth] Вы зарегистрированы!");

            ModelUser model;
            db.HasUser(player.Username, out model);

            users.Add(new User { ip = player.EndPoint.Address.ToString(), player = player, model = model, nick = player.Username });
        }

        private void ProcessLogin(Player player, ModelUser model)
        {
            // авторизация была?
            if (users.Exists(x => x.ip == player.EndPoint.Address.ToString() && x.nick == player.Username))
            {
                // заменяем элемент, дабы порт сменился
                int i = users.FindIndex(x => x.ip == player.EndPoint.Address.ToString() && x.nick == player.Username);
                users[i] = users[i].Copy(player: player);
                // просто пропускаем его, ip тот-же
                player.AddPopup(new Popup()
                {
                    MessageType = MessageType.Tip,
                    Message = "§2Авторизация успешна!",
                    Duration = 20 * 4
                });

                player.isLogin = true;
            }
            else
            {
                var pos = player.KnownPosition;
                preLoginUser[player.Username] = model.password;
                Timer timer = null;
                timer = new Timer(((o) =>
                {
                    if (player.isLogin)
                    {
                        timer.Change(Timeout.Infinite, Timeout.Infinite);
                        timer.Dispose();
                        return;
                    }

                    player.AddPopup(new Popup()
                    {
                        MessageType = MessageType.Tip,
                        Message = "§cВойдите используя команду /l или /login",
                        Duration = 100
                    });

                    // не даем двигатся (инвента и так нет)
                    player.SetPosition(pos);

                    
                }), null, 0, 100);
            }
        }

        private void ProcessRegister(Player player)
        {
            var pos = player.KnownPosition;
            Timer timer = null;
            timer = new Timer(((o) =>
            {
                if (player.isLogin)
                {
                    timer.Change(Timeout.Infinite, Timeout.Infinite);
                    timer.Dispose();
                    return;
                }

                player.AddPopup(new Popup()
                {
                    MessageType = MessageType.Tip,
                    Message = "§cЗарегистрируйтесь используя команду /r или /register",
                    Duration = 100
                });

                // не даем двигатся (инвента и так нет)
                player.SetPosition(pos);


            }), null, 0, 100);
        }

        #endregion

        [Command(Command = "tp", RoleRequired = 1)]
        public void Teleport(Player player)
        {
            player.SendMessage("tp....");
        }
    }
}
