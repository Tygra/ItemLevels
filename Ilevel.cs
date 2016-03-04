using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;

using Terraria;
using TerrariaApi.Server;

using TShockAPI;
using TShockAPI.DB;
using TShockAPI.Hooks;

using Mono.Data.Sqlite;
using MySql.Data.MySqlClient;

namespace ItemLevels
{
    [ApiVersion(1, 22)]
    public class Ilevel : TerrariaPlugin
    {
        public IDbConnection Database;
        public string SavePath = TShock.SavePath;

        public override string Name
        { get { return "RPG Commands"; } }

        public override string Author
        { get { return "Tygra"; } }

        public override string Description
        { get { return "Geldar RPG Commads"; } }

        public override Version Version
        { get { return new Version(1, 0); } }

        public Ilevel(Main game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            ServerApi.Hooks.GameInitialize.Register(this, OnInitialize);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ServerApi.Hooks.GameInitialize.Deregister(this, OnInitialize);
                Database.Dispose();
            }
            base.Dispose(disposing);
        }

        private void OnInitialize(EventArgs args)
        {
            switch (TShock.Config.StorageType.ToLower())
            {
                case "mysql":
                    string[] host = TShock.Config.MySqlHost.Split(':');
                    Database = new MySqlConnection()
                    {
                        ConnectionString = string.Format("Server={0}; Port={1}; Database={2}; Uid={3}; Pwd={4};",
                        host[0],
                        host.Length == 1 ? "3306" : host[1],
                        TShock.Config.MySqlDbName,
                        TShock.Config.MySqlUsername,
                        TShock.Config.MySqlPassword)
                    };
                    break;

                case "sqlite":
                    if (!System.IO.Directory.Exists(SavePath))
                    {
                        System.IO.Directory.CreateDirectory(SavePath);
                    }

                    string sql = Path.Combine(SavePath, "ilevel.sqlite");
                    Database = new SqliteConnection(string.Format("uri=file://{0},Version=3", sql));
                    break;
            }

            SqlTableCreator sqlcreator = new SqlTableCreator(Database, Database.GetSqlType() == SqlType.Sqlite ? (IQueryBuilder)new SqliteQueryCreator() : new MysqlQueryCreator());
            sqlcreator.EnsureTableStructure(new SqlTable("ilevel",
                new SqlColumn("ID", MySqlDbType.Int32) { Primary = true, AutoIncrement = true },
                new SqlColumn("ItemName", MySqlDbType.Text) { Length = 30 },
                new SqlColumn("Restriction", MySqlDbType.Text) { Length = 60 }             
                ));

            Commands.ChatCommands.Add(new Command(Itemlevel, "ilevel"));
            Commands.ChatCommands.Add(new Command("geldar.admin", ILadmin, "iladmin"));
        }

        private void Itemlevel(CommandArgs args)
        {            

        }

        private void ILadmin(CommandArgs args)
        {
            if (args.Parameters.Count < 1)
            {
                args.Player.SendMessage("Available subcmds <add/del>", Color.Goldenrod);
            }

            switch (args.Parameters[0])
            {
                case "add":
                    {

                    }
                    break;

                case "del":
                    {

                    }
                    break;
            }
        }
    }
}
