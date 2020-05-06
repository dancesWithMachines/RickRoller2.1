using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RickRoller_2
{
    public partial class Form1 : Form
    {
        Backend backend;
        private string path;
        private string[] friends;
        public Form1()
        {
            InitializeComponent();
            passwordTextBox.PasswordChar = '*';
        }

        private void showDialog(object sender, EventArgs e)
        {
            filePath.Filter = "Pliki tekstowe (*.txt)|*.txt";
            if (filePath.ShowDialog() == DialogResult.OK)
            {
                path = filePath.FileName;
            }
        }

        private void tryLogging(object sender, EventArgs e)
        {
            if (loginTextBox.Text != "" && passwordTextBox.Text != "")
            {
                backend = new Backend();
                backend.login(loginTextBox.Text, passwordTextBox.Text);
            }
            else
            {
                setStatus("Fill in required fields");
            }
        }

        private void getFriendsLooser(object sender, EventArgs e)
        {
            if (backend != null)
            {
                setStatus("Getting friends list");
                friends = backend.getFriendsList();
                if (friends.Length > 0)
                {
                    foreach (string element in friends)
                    {
                        friendsList.Items.Add(element);
                    }
                }
                setStatus("Getting friends list, done");
            }
            else
                setStatus("You must login first");
        }

        private void rickRollAllFriends(object sender, EventArgs e)
        {
            if (backend != null)
            {
                if (friends.Length < 0)
                    getFriendsLooser(sender, e);
                setStatus("RickRolling all friends");
                foreach (string name in friends)
                {
                    backend.rickRoll(name, path);
                }
                setStatus("RickRolling all friends, done");
            }
            else
                setStatus("You must login first");

        }

        private void rickRollFriend(object sender, EventArgs e)
        {
            if (pathTextBox.Text != "" && path != null)
            {
                if (backend != null)
                    backend.rickRoll(pathTextBox.Text, path);
                else
                    setStatus("You must login first");
            }
            else
            {
                setStatus("Fill in required fields");
            }
        }


        void selectFriend(object sender, MouseEventArgs e)
        {
            int index = this.friendsList.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                pathTextBox.Text = friendsList.Items[index].ToString();
            }
        }

        private void setStatus(string status)
        {
            statusLabel.Text = status;
        }

    }
}
