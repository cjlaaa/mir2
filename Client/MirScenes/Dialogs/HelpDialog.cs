using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client.MirControls;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirObjects;
using Client.MirSounds;
using S = ServerPackets;
using C = ClientPackets;

namespace Client.MirScenes.Dialogs
{
    public sealed class HelpDialog : MirImageControl
    {
        public List<HelpPage> Pages = new List<HelpPage>();

        public MirButton CloseButton, NextButton, PreviousButton;
        public MirLabel PageLabel;
        public HelpPage CurrentPage;

        public int CurrentPageNumber = 0;

        public HelpDialog()
        {
            Index = 920;
            Library = Libraries.Prguse;
            Movable = true;
            Sort = true;

            Location = Center;

            MirImageControl TitleLabel = new MirImageControl
            {
                Index = 57,
                Library = Libraries.Title,
                Location = new Point(18, 9),
                Parent = this
            };

            PreviousButton = new MirButton
            {
                Index = 240,
                HoverIndex = 241,
                PressedIndex = 242,
                Library = Libraries.Prguse2,
                Parent = this,
                Size = new Size(16, 16),
                Location = new Point(210, 485),
                Sound = SoundList.ButtonA,
            };
            PreviousButton.Click += (o, e) =>
            {
                CurrentPageNumber--;

                if (CurrentPageNumber < 0) CurrentPageNumber = Pages.Count - 1;

                DisplayPage(CurrentPageNumber);
            };

            NextButton = new MirButton
            {
                Index = 243,
                HoverIndex = 244,
                PressedIndex = 245,
                Library = Libraries.Prguse2,
                Parent = this,
                Size = new Size(16, 16),
                Location = new Point(310, 485),
                Sound = SoundList.ButtonA,
            };
            NextButton.Click += (o, e) =>
            {
                CurrentPageNumber++;

                if (CurrentPageNumber > Pages.Count - 1) CurrentPageNumber = 0;

                DisplayPage(CurrentPageNumber);
            };

            PageLabel = new MirLabel
            {
                Text = "",
                Font = new Font(Settings.FontName, 9F),
                DrawFormat = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter,
                Parent = this,
                NotControl = true,
                Location = new Point(230, 480),
                Size = new Size(80, 20)
            };

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(509, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            LoadImagePages();

            DisplayPage();
        }

        private void LoadImagePages()
        {
            Point location = new Point(12, 35);

            Dictionary<string, string> keybinds = new Dictionary<string, string>();

            List<HelpPage> imagePages = new List<HelpPage> { 
                new HelpPage("快捷键", -1, new ShortcutPage1 { Parent = this } ) { Parent = this, Location = location, Visible = false }, 
                new HelpPage("快捷键", -1, new ShortcutPage2 { Parent = this } ) { Parent = this, Location = location, Visible = false }, 
                new HelpPage("聊天快捷键", -1, new ShortcutPage3 { Parent = this } ) { Parent = this, Location = location, Visible = false }, 
                new HelpPage("移动", 0, null) { Parent = this, Location = location, Visible = false }, 
                new HelpPage("攻击", 1, null) { Parent = this, Location = location, Visible = false }, 
                new HelpPage("收集物品", 2, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("生命值", 3, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("技能", 4, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("技能", 5, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("魔法值", 6, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("聊天", 7, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("编组", 8, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("持久力", 9, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("购买物品", 10, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("出售物品", 11, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("修理物品", 12, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("交易", 13, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("检查", 14, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("统计", 15, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("统计", 16, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("统计", 17, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("统计", 18, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("统计", 19, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("统计", 20, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("任务系统", 21, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("任务系统", 22, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("任务系统", 23, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("任务系统", 24, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("坐骑系统", 25, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("坐骑系统", 26, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("钓鱼系统", 27, null) { Parent = this, Location = location, Visible = false },
                new HelpPage("宝石和宝珠", 28, null) { Parent = this, Location = location, Visible = false },
            };

            Pages.AddRange(imagePages);
        }


        public void DisplayPage(string pageName)
        {
            if (Pages.Count < 1) return;

            for (int i = 0; i < Pages.Count; i++)
            {
                if (Pages[i].Title.ToLower() != pageName.ToLower()) continue;

                DisplayPage(i);
                break;
            }
        }

        public void DisplayPage(int id = 0)
        {
            if (Pages.Count < 1) return;

            if (id > Pages.Count - 1) id = Pages.Count - 1;
            if (id < 0) id = 0;

            if (CurrentPage != null)
            {
                CurrentPage.Visible = false;
                if (CurrentPage.Page != null) CurrentPage.Page.Visible = false;
            }

            CurrentPage = Pages[id];

            if (CurrentPage == null) return;

            CurrentPage.Visible = true;
            if (CurrentPage.Page != null) CurrentPage.Page.Visible = true;
            CurrentPageNumber = id;

            CurrentPage.PageTitleLabel.Text = id + 1 + ". " + CurrentPage.Title;

            PageLabel.Text = string.Format("{0} / {1}", id + 1, Pages.Count);

            Show();
        }

        public void Show()
        {
            if (Visible) return;
            Visible = true;
        }

        public void Hide()
        {
            if (!Visible) return;
            Visible = false;
        }

        public void Toggle()
        {
            if (!Visible)
                Show();
            else
                Hide();
        }
    }

    public class ShortcutPage1 : ShortcutInfoPage
    {
        public ShortcutPage1()
        {
            Shortcuts = new List<ShortcutInfo>();
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Exit), "退出游戏"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Logout), "返回角色界面"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Bar1Skill1) + "-" + CMain.InputKeys.GetKey(KeybindOptions.Bar1Skill8), "技能键"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Inventory), "物品界面(打开/关闭)"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Equipment), "角色界面(打开/关闭)"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Skills), "技能界面(打开/关闭)"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Group), "编组界面(打开/关闭)"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Trade), "交易界面(打开/关闭)"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Friends), "好友界面(打开/关闭)"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Minimap), "小地图(打开/关闭)"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Guilds), "行会界面(打开/关闭)"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.GameShop), "游戏商城(打开/关闭)"));
            //Shortcuts.Add(new ShortcutInfo("K", "Rental window (open / close)"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Relationship), "婚姻界面(打开/关闭)"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Belt), "腰带界面(打开/关闭)"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Options), "选项界面(打开/关闭)"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Help), "帮助界面(打开/关闭)"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Mount), "上坐骑/下坐骑"));

            LoadKeyBinds();
        }
    }
    public class ShortcutPage2 : ShortcutInfoPage
    {
        public ShortcutPage2()
        {
            Shortcuts = new List<ShortcutInfo>();
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.ChangePetmode), "切换宠物攻击/移动模式"));
            //Shortcuts.Add(new ShortcutInfo("Ctrl + F", "Change the font in the chat box"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.ChangeAttackmode), "切换玩家攻击模式"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.AttackmodePeace), "和平模式 - 攻击时只会伤害到怪物。"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.AttackmodeGroup), "编组模式 - 攻击时不会伤害到一起组队的玩家。"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.AttackmodeGuild), "行会模式 - 攻击时不会伤害到同行会的玩家"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.AttackmodeRedbrown), "善恶模式 - 攻击时只会伤害到怪物和犯罪玩家"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.AttackmodeAll), "全体模式 - 攻击时会伤害到所有对象"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Bigmap), "大地图(打开/关闭)"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Skillbar), "技能条(打开/关闭)"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Autorun), "自动奔跑(打开/关闭)"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Cameramode), "摄影模式(打开/关闭)"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Pickup), "高亮显示物品/拾取物品"));
            Shortcuts.Add(new ShortcutInfo("Ctrl+鼠标右键", "查看其它玩家装备"));
            //Shortcuts.Add(new ShortcutInfo("F12", "Chat macros"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Screenshot), "截图"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Fishing), "钓鱼窗口(打开/关闭)"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Mentor), "师徒窗口(打开/关闭)"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.CreaturePickup), "宠物拾取(多鼠标目标)"));
            Shortcuts.Add(new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.CreatureAutoPickup), "宠物拾取(单鼠标目标)"));

            LoadKeyBinds();
        }
    }
    public class ShortcutPage3 : ShortcutInfoPage
    {
        public ShortcutPage3()
        {
            Shortcuts = new List<ShortcutInfo>();
            //Shortcuts.Add(new ShortcutInfo("` / Ctrl", "Change the skill bar"));
            Shortcuts.Add(new ShortcutInfo("/(玩家名)", "私聊其他玩家"));
            Shortcuts.Add(new ShortcutInfo("!(消息)", "大喊命令,附近玩家会收到消息。"));
            Shortcuts.Add(new ShortcutInfo("!~(消息)", "行会聊天命令"));

            LoadKeyBinds();
        }
    }

    public class ShortcutInfo
    {
        public string Shortcut { get; set; }
        public string Information { get; set; }

        public ShortcutInfo(string shortcut, string info)
        {
            Shortcut = shortcut.Replace("\n", " + ");
            Information = info;
        }
    }

    public class ShortcutInfoPage : MirControl
    {
        protected List<ShortcutInfo> Shortcuts = new List<ShortcutInfo>();

        public ShortcutInfoPage()
        {
            Visible = false;

            MirLabel shortcutTitleLabel = new MirLabel
            {
                Text = "快捷键",
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                ForeColour = Color.White,
                Font = new Font(Settings.FontName, 10F),
                Parent = this,
                AutoSize = true,
                Location = new Point(13, 75),
                Size = new Size(100, 30)
            };

            MirLabel infoTitleLabel = new MirLabel
            {
                Text = "信息",
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                ForeColour = Color.White,
                Font = new Font(Settings.FontName, 10F),
                Parent = this,
                AutoSize = true,
                Location = new Point(114, 75),
                Size = new Size(405, 30)
            };
        }

        public void LoadKeyBinds()
        {
            if (Shortcuts == null) return;

            for (int i = 0; i < Shortcuts.Count; i++)
            {
                MirLabel shortcutLabel = new MirLabel
                {
                    Text = Shortcuts[i].Shortcut,
                    ForeColour = Color.Yellow,
                    DrawFormat = TextFormatFlags.VerticalCenter,
                    Font = new Font(Settings.FontName, 9F),
                    Parent = this,
                    AutoSize = true,
                    Location = new Point(18, 107 + (20 * i)),
                    Size = new Size(95, 23),
                };

                MirLabel informationLabel = new MirLabel
                {
                    Text = Shortcuts[i].Information,
                    DrawFormat = TextFormatFlags.VerticalCenter,
                    ForeColour = Color.White,
                    Font = new Font(Settings.FontName, 9F),
                    Parent = this,
                    AutoSize = true,
                    Location = new Point(119, 107 + (20 * i)),
                    Size = new Size(400, 23),
                };
            }  
        }
    }

    public class HelpPage : MirControl
    {
        public string Title;
        public int ImageID;
        public MirControl Page;

        public MirLabel PageTitleLabel;

        public HelpPage(string title, int imageID, MirControl page)
        {
            Title = title;
            ImageID = imageID;
            Page = page;

            NotControl = true;
            Size = new System.Drawing.Size(508, 396 + 40);

            BeforeDraw += HelpPage_BeforeDraw;

            PageTitleLabel = new MirLabel
            {
                Text = Title,
                Font = new Font(Settings.FontName, 10F, FontStyle.Bold),
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                Parent = this,
                Size = new System.Drawing.Size(242, 30),
                Location = new Point(135, 4)
            };
        }

        void HelpPage_BeforeDraw(object sender, EventArgs e)
        {
            if (ImageID < 0) return;

            Libraries.Help.Draw(ImageID, new Point(DisplayLocation.X, DisplayLocation.Y + 40), Color.White, false);
        }
    }
}
