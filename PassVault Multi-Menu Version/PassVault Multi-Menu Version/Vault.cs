using System;
using System.Data;
using System.Data.OleDb;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace PassVault_Multi_Menu_Version
{
    public partial class Vault : Form
    {

        OleDbConnection connection = new OleDbConnection("Provider=Microsoft.ACE.OleDb.16.0; Data Source = D:\\PassVault\\PassVaultDatabase.accdb");

        OleDbCommand p_command;
        OleDbCommand ba_command;
        OleDbCommand c_command;
        OleDbCommand a_command;
        OleDbCommand n_command;

        OleDbDataAdapter p_dataAdapter;
        OleDbDataAdapter ba_dataAdapter;
        OleDbDataAdapter c_dataAdapter;
        OleDbDataAdapter a_dataAdapter;
        OleDbDataAdapter n_dataAdapter;

        DataTable p_dataTable = new DataTable();
        DataTable ba_dataTable = new DataTable();
        DataTable c_dataTable = new DataTable();
        DataTable a_dataTable = new DataTable();
        DataTable n_dataTable = new DataTable();

        bool editingPassword = false;
        bool editingBankAccount = false;
        bool editingCard = false;
        bool editingAddress = false;
        bool editingNote = false;



        void GetPasswords()
        {
            p_dataAdapter = new OleDbDataAdapter("SELECT *FROM Passwords", connection);
            connection.Open();
            p_dataAdapter.Fill(p_dataTable);
            p_dataGridView.DataSource = p_dataTable;
            connection.Close();
        }

        void GetBankAccounts()
        {
            ba_dataAdapter = new OleDbDataAdapter("SELECT *FROM BankAccounts", connection);
            connection.Open();
            ba_dataAdapter.Fill(ba_dataTable);
            ba_dataGridView.DataSource = ba_dataTable;
            connection.Close();
        }

        void GetCards()
        {
            c_dataAdapter = new OleDbDataAdapter("SELECT *FROM Cards", connection);
            connection.Open();
            c_dataAdapter.Fill(c_dataTable);
            c_dataGridView.DataSource = c_dataTable;
            connection.Close();
        }

        void GetAddresses()
        {
            a_dataAdapter = new OleDbDataAdapter("SELECT *FROM Addresses", connection);
            connection.Open();
            a_dataAdapter.Fill(a_dataTable);
            a_dataGridView.DataSource = a_dataTable;
            connection.Close();
        }

        void GetNotes()
        {
            n_dataAdapter = new OleDbDataAdapter("SELECT *FROM Notes", connection);
            connection.Open();
            n_dataAdapter.Fill(n_dataTable);
            n_dataGridView.DataSource = n_dataTable;
            connection.Close();
        }



        public Vault()
        {
            InitializeComponent();
        }



        private void PassVault_Load(object sender, EventArgs e)
        {
            GetPasswords();

            GetBankAccounts();

            GetCards();

            GetAddresses();

            GetNotes();
        }



        // for notes
        private void n_buttonDelete_Click(object sender, EventArgs e)
        {
            try 
            { 
                string query = "Delete FROM Notes WHERE NoteID=@NoteID";

                n_command = new OleDbCommand(query, connection);
                n_command.Parameters.AddWithValue("@NoteID", Convert.ToInt32(n_dataTable.Rows[n_dataGridView.CurrentCell.RowIndex]["NoteID"]));

                connection.Open();
                n_command.ExecuteNonQuery();
                connection.Close();

                n_dataTable.Clear();
                GetNotes();
            }
            catch (Exception) { MessageBox.Show("Select row to delete."); }
        }

        private void n_dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                n_textBoxTitle.Text = n_dataTable.Rows[n_dataGridView.CurrentCell.RowIndex].ItemArray[1].ToString();
                n_textBoxNotes.Text = n_dataTable.Rows[n_dataGridView.CurrentCell.RowIndex].ItemArray[2].ToString();
                editingNote = true;
            }
            catch (Exception)
            { MessageBox.Show("Selected empty row."); }
        }

        private void n_buttonNew_Click(object sender, EventArgs e)
        {
            n_textBoxTitle.Text = ""; n_textBoxNotes.Text = "";
            editingNote = false;
        }

        private void n_buttonSave_Click(object sender, EventArgs e) //if existing item is being overwritten
        {
            if (editingNote == true)
            {
                string query = "UPDATE Notes SET n_Title=@n_Title, n_Notes=@n_Notes " +
                    "WHERE NoteID=@NoteID";

                n_command = new OleDbCommand(query, connection);
                n_command.Parameters.AddWithValue("@n_Title", n_textBoxTitle.Text);
                n_command.Parameters.AddWithValue("@n_Notes", n_textBoxNotes.Text);
                n_command.Parameters.AddWithValue("@NoteID", Convert.ToInt32(n_dataTable.Rows[n_dataGridView.CurrentCell.RowIndex]["NoteID"]));
                
                connection.Open();
                n_command.ExecuteNonQuery();
                connection.Close();

                n_dataTable.Clear();
                GetNotes();
            }
            else //if the item is new
            {
                string query = "INSERT INTO Notes (n_Title,n_Notes) VALUES (@n_Title,@n_Notes)";

                n_command = new OleDbCommand(query, connection);
                n_command.Parameters.AddWithValue("@n_Title", n_textBoxTitle.Text);
                n_command.Parameters.AddWithValue("@n_Notes", n_textBoxNotes.Text);

                connection.Open();
                n_command.ExecuteNonQuery();
                connection.Close();

                n_dataTable.Clear();
                GetNotes();
            }

            n_textBoxTitle.Text = ""; n_textBoxNotes.Text = "";
            editingNote = false;
        }



        // for passwords
        private void p_buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "Delete FROM Passwords WHERE PasswordID=@PasswordID";

                p_command = new OleDbCommand(query, connection);
                p_command.Parameters.AddWithValue("@PasswordID", Convert.ToInt32(p_dataTable.Rows[p_dataGridView.
                    CurrentCell.RowIndex]["PasswordID"]));

                connection.Open();
                p_command.ExecuteNonQuery();
                connection.Close();

                p_dataTable.Clear();
                GetPasswords();
            }
            catch (Exception) { MessageBox.Show("Select row to delete."); }
        }

        private void p_dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                p_textBoxTitle.Text = p_dataTable.Rows[p_dataGridView.CurrentCell.RowIndex].ItemArray[1].ToString();
                p_textBoxUsername.Text = p_dataTable.Rows[p_dataGridView.CurrentCell.RowIndex].ItemArray[2].ToString();
                p_textBoxPassword.Text = p_dataTable.Rows[p_dataGridView.CurrentCell.RowIndex].ItemArray[3].ToString();
                p_textBoxLink.Text = p_dataTable.Rows[p_dataGridView.CurrentCell.RowIndex].ItemArray[4].ToString();
                editingPassword = true;
            }
            catch (Exception) { MessageBox.Show("Selected no row / empty row."); }
        }

        private void p_buttonNew_Click(object sender, EventArgs e)
        {
            p_textBoxTitle.Text = ""; p_textBoxUsername.Text = ""; p_textBoxPassword.Text = ""; p_textBoxLink.Text = "";
            editingPassword = false;
        }

        private void p_buttonSave_Click(object sender, EventArgs e)
        {
            if (editingPassword == true) //if existing item is being overwritten
            {
                string query = "UPDATE Passwords SET p_Title=@p_Title, p_Username=@p_Username, p_Password=@p_Password, p_Link=@p_Link " + 
                    "WHERE PasswordID=@PasswordID";

                p_command = new OleDbCommand(query, connection);
                p_command.Parameters.AddWithValue("@p_Title", p_textBoxTitle.Text);
                p_command.Parameters.AddWithValue("@p_Username", p_textBoxUsername.Text);
                p_command.Parameters.AddWithValue("@p_Password", p_textBoxPassword.Text);
                p_command.Parameters.AddWithValue("@p_Link", p_textBoxLink.Text);
                p_command.Parameters.AddWithValue("@PasswordID", Convert.ToInt32(p_dataTable.Rows[p_dataGridView.CurrentCell.RowIndex]["PasswordID"]));
                connection.Open();
                p_command.ExecuteNonQuery();
                connection.Close();

                p_dataTable.Clear();
                GetPasswords();
            }
            else //if the item is new
            {
                string query = "INSERT INTO Passwords (p_Title, p_Username, p_Password, p_Link) VALUES (@p_Title, @p_Username, @p_Password, @p_Link)";

                p_command = new OleDbCommand(query, connection);
                p_command.Parameters.AddWithValue("@p_Title", p_textBoxTitle.Text);
                p_command.Parameters.AddWithValue("@p_Username", p_textBoxUsername.Text);
                p_command.Parameters.AddWithValue("@p_Password", toHash256(p_textBoxPassword.Text));
                p_command.Parameters.AddWithValue("@p_Link", p_textBoxLink.Text);

                connection.Open();
                p_command.ExecuteNonQuery();
                connection.Close();

                p_dataTable.Clear();
                GetPasswords();
            }

            p_textBoxTitle.Text = ""; p_textBoxUsername.Text = ""; p_textBoxPassword.Text = ""; p_textBoxLink.Text = "";
            editingPassword = false;

        }



        // for bank account
        private void ba_buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "Delete FROM BankAccounts WHERE BankAccountID=@BankAccountID";

                ba_command = new OleDbCommand(query, connection);
                ba_command.Parameters.AddWithValue("@BankAccountID", Convert.ToInt32(ba_dataTable.Rows[ba_dataGridView.
                    CurrentCell.RowIndex]["BankAccountID"]));

                connection.Open();
                ba_command.ExecuteNonQuery();
                connection.Close();

                ba_dataTable.Clear();
                GetBankAccounts();
            }
            catch (Exception) { MessageBox.Show("Select row to delete."); }
        }

        private void ba_dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ba_textBoxTitle.Text = ba_dataTable.Rows[ba_dataGridView.CurrentCell.RowIndex].ItemArray[1].ToString();
                ba_textBoxUsername.Text = ba_dataTable.Rows[ba_dataGridView.CurrentCell.RowIndex].ItemArray[2].ToString();
                ba_textBoxPassword.Text = ba_dataTable.Rows[ba_dataGridView.CurrentCell.RowIndex].ItemArray[3].ToString();
                ba_textBoxAccNo.Text = ba_dataTable.Rows[ba_dataGridView.CurrentCell.RowIndex].ItemArray[4].ToString();
                ba_textBoxSortCode.Text = ba_dataTable.Rows[ba_dataGridView.CurrentCell.RowIndex].ItemArray[5].ToString();
                ba_textBoxLink.Text = ba_dataTable.Rows[ba_dataGridView.CurrentCell.RowIndex].ItemArray[6].ToString();
                editingBankAccount = true;
            }
            catch (Exception) { MessageBox.Show("Selected empty row."); }
        }

        private void ba_buttonNew_Click(object sender, EventArgs e)
        {
            ba_textBoxTitle.Text = ""; ba_textBoxUsername.Text = ""; ba_textBoxPassword.Text = "";
            ba_textBoxAccNo.Text = ""; ba_textBoxSortCode.Text = ""; ba_textBoxLink.Text = "";
            editingBankAccount = false;
        }

        private void ba_buttonSave_Click(object sender, EventArgs e) //if existing item is being overwritten
        {
            if (editingBankAccount == true)
            {
                string query = "UPDATE BankAccounts SET ba_Title=@ba_Title, ba_Username=@ba_Username, ba_Password=@ba_Password, " +
                    "ba_AccountNumber=@ba_AccountNumber, ba_SortCode=@ba_SortCode, ba_Link=@ba_Link " +
                    "WHERE BankAccountID=@BankAccountID";

                ba_command = new OleDbCommand(query, connection);
                ba_command.Parameters.AddWithValue("@ba_Title", ba_textBoxTitle.Text);
                ba_command.Parameters.AddWithValue("@ba_Username", ba_textBoxUsername.Text);
                ba_command.Parameters.AddWithValue("@ba_Password", ba_textBoxPassword.Text);
                ba_command.Parameters.AddWithValue("@ba_AccountNumber", ba_textBoxAccNo.Text);
                ba_command.Parameters.AddWithValue("@ba_SortCode", ba_textBoxSortCode.Text);
                ba_command.Parameters.AddWithValue("@ba_Link", ba_textBoxLink.Text);
                ba_command.Parameters.AddWithValue("@BankAccountID", Convert.ToInt32(ba_dataTable.Rows[ba_dataGridView.
                    CurrentCell.RowIndex]["BankAccountID"]));

                connection.Open();
                ba_command.ExecuteNonQuery();
                connection.Close();

                ba_dataTable.Clear();
                GetBankAccounts();
            }
            else //if the item is new
            {
                string query = "INSERT INTO BankAccounts (ba_Title, ba_Username, ba_Password, ba_AccountNumber, ba_SortCode, ba_Link) " +
                    "VALUES (@ba_Title, @ba_Username, @ba_Password, @ba_AccountNumber, @ba_SortCode, @ba_Link)";

                ba_command = new OleDbCommand(query, connection);
                ba_command.Parameters.AddWithValue("@ba_Title", ba_textBoxTitle.Text);
                ba_command.Parameters.AddWithValue("@ba_Username", ba_textBoxUsername.Text);
                ba_command.Parameters.AddWithValue("@ba_Password", ba_textBoxPassword.Text);
                ba_command.Parameters.AddWithValue("@ba_AccountNumber", ba_textBoxAccNo.Text);
                ba_command.Parameters.AddWithValue("@ba_SortCode", ba_textBoxSortCode.Text);
                ba_command.Parameters.AddWithValue("@ba_Link", ba_textBoxLink.Text);
                connection.Open();
                ba_command.ExecuteNonQuery();
                connection.Close();

                ba_dataTable.Clear();
                GetBankAccounts();
            }
            
            ba_textBoxTitle.Text = ""; ba_textBoxUsername.Text = ""; ba_textBoxPassword.Text = "";
            ba_textBoxAccNo.Text = ""; ba_textBoxSortCode.Text = ""; ba_textBoxLink.Text = "";
            editingBankAccount = false;
        }



        // for cards
        private void c_buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "Delete FROM Cards WHERE CardID=@CardID";

                c_command = new OleDbCommand(query, connection);
                c_command.Parameters.AddWithValue("@CardID", Convert.ToInt32(c_dataTable.Rows[c_dataGridView.
                    CurrentCell.RowIndex]["CardID"]));

                connection.Open();
                c_command.ExecuteNonQuery();
                connection.Close();

                c_dataTable.Clear();
                GetCards();
            }
            catch (Exception) { MessageBox.Show("Select row to delete."); }
        }

        private void c_dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                c_textBoxTitle.Text = c_dataTable.Rows[c_dataGridView.CurrentCell.RowIndex].ItemArray[1].ToString();
                c_textBoxNameOnCard.Text = c_dataTable.Rows[c_dataGridView.CurrentCell.RowIndex].ItemArray[2].ToString();
                c_textBoxCardNo.Text = c_dataTable.Rows[c_dataGridView.CurrentCell.RowIndex].ItemArray[3].ToString();
                c_textBoxCardType.Text = c_dataTable.Rows[c_dataGridView.CurrentCell.RowIndex].ItemArray[4].ToString();
                c_textBoxSecCode.Text = c_dataTable.Rows[c_dataGridView.CurrentCell.RowIndex].ItemArray[5].ToString();
                c_textBoxStartDate.Text = c_dataTable.Rows[c_dataGridView.CurrentCell.RowIndex].ItemArray[6].ToString();
                c_textBoxEndDate.Text = c_dataTable.Rows[c_dataGridView.CurrentCell.RowIndex].ItemArray[7].ToString();
                c_textBoxLink.Text = c_dataTable.Rows[c_dataGridView.CurrentCell.RowIndex].ItemArray[8].ToString();
                editingCard = true;
            }
            catch (Exception) { MessageBox.Show("Selected empty row."); }
        }

        private void c_buttonNew_Click(object sender, EventArgs e)
        {
            c_textBoxTitle.Text = ""; c_textBoxNameOnCard.Text = ""; c_textBoxCardNo.Text = ""; c_textBoxCardType.Text = "";
            c_textBoxSecCode.Text = ""; c_textBoxStartDate.Text = ""; c_textBoxEndDate.Text = ""; c_textBoxLink.Text = "";
            editingCard = false;
        }

        private void c_buttonSave_Click(object sender, EventArgs e)
        {
            if (editingCard == true) //if existing item is being overwritten
            {
                string query = "UPDATE Cards SET c_Title=@c_Title, c_NameOnCard=@c_NameOnCard, c_CardNumber=@c_CardNumber, " +
                    "c_CardType=@c_CardType, c_SecurityCode=@c_SecurityCode, c_StartDate=@c_StartDate, c_EndDate=@c_EndDate, c_Link=@c_Link " +
                    "WHERE CardID=@CardID";

                c_command = new OleDbCommand(query, connection);
                c_command.Parameters.AddWithValue("@c_Title", c_textBoxTitle.Text);
                c_command.Parameters.AddWithValue("@c_NameOnCard", c_textBoxNameOnCard.Text);
                c_command.Parameters.AddWithValue("@c_CardNumber", c_textBoxCardNo.Text);
                c_command.Parameters.AddWithValue("@c_CardType", c_textBoxCardType.Text);
                c_command.Parameters.AddWithValue("@c_SecurityCode", c_textBoxSecCode.Text);
                c_command.Parameters.AddWithValue("@c_StartDate", c_textBoxStartDate.Text);
                c_command.Parameters.AddWithValue("@c_EndDate", c_textBoxEndDate.Text);
                c_command.Parameters.AddWithValue("@c_Link", c_textBoxLink.Text);
                c_command.Parameters.AddWithValue("@CardID", Convert.ToInt32(c_dataTable.Rows[c_dataGridView.CurrentCell.
                    RowIndex]["CardID"]));
                connection.Open();
                c_command.ExecuteNonQuery();
                connection.Close();

                c_dataTable.Clear();
                GetCards();
            }
            else //if the item is new
            {
                string query = "INSERT INTO Cards (c_Title, c_NameOnCard, c_CardNumber, c_CardType, c_SecurityCode, c_StartDate, " +
                    "c_EndDate, c_Link) " +
                    "VALUES (@c_Title, @c_NameOnCard, @c_CardNumber, @c_CardType, @c_SecurityCode, @c_StartDate, @c_EndDate, @c_Link)";

                c_command = new OleDbCommand(query, connection);
                c_command.Parameters.AddWithValue("@c_Title", c_textBoxTitle.Text);
                c_command.Parameters.AddWithValue("@c_NameOnCard", c_textBoxNameOnCard.Text);
                c_command.Parameters.AddWithValue("@c_CardNumber", c_textBoxCardNo.Text);
                c_command.Parameters.AddWithValue("@c_CardType", c_textBoxCardType.Text);
                c_command.Parameters.AddWithValue("@c_SecurityCode", c_textBoxSecCode.Text);
                c_command.Parameters.AddWithValue("@c_StartDate", c_textBoxStartDate.Text);
                c_command.Parameters.AddWithValue("@c_EndDate", c_textBoxEndDate.Text);
                c_command.Parameters.AddWithValue("@c_Link", c_textBoxLink.Text);
                connection.Open();
                c_command.ExecuteNonQuery();
                connection.Close();

                c_dataTable.Clear();
                GetCards();
            }

            c_textBoxTitle.Text = ""; c_textBoxNameOnCard.Text = ""; c_textBoxCardNo.Text = ""; c_textBoxCardType.Text = "";
            c_textBoxSecCode.Text = ""; c_textBoxStartDate.Text = ""; c_textBoxEndDate.Text = ""; c_textBoxLink.Text = "";
            editingCard = false;
        }



        // for addresses
        private void a_buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "Delete FROM Addresses WHERE AddressID=@AddressID";

                a_command = new OleDbCommand(query, connection);
                a_command.Parameters.AddWithValue("@AddressID", Convert.ToInt32(a_dataTable.Rows[a_dataGridView.
                    CurrentCell.RowIndex]["AddressID"]));

                connection.Open();
                a_command.ExecuteNonQuery();
                connection.Close();

                a_dataTable.Clear();
                GetAddresses();
            }
            catch (Exception) { MessageBox.Show("Select row to delete."); }
        }

        private void a_dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                a_textBoxTitle.Text = a_dataTable.Rows[a_dataGridView.CurrentCell.RowIndex].ItemArray[1].ToString();
                a_textBoxAddress1.Text = a_dataTable.Rows[a_dataGridView.CurrentCell.RowIndex].ItemArray[2].ToString();
                a_textBoxAddress2.Text = a_dataTable.Rows[a_dataGridView.CurrentCell.RowIndex].ItemArray[3].ToString();
                a_textBoxAddress3.Text = a_dataTable.Rows[a_dataGridView.CurrentCell.RowIndex].ItemArray[4].ToString();
                a_textBoxCity.Text = a_dataTable.Rows[a_dataGridView.CurrentCell.RowIndex].ItemArray[5].ToString();
                a_textBoxCounty.Text = a_dataTable.Rows[a_dataGridView.CurrentCell.RowIndex].ItemArray[6].ToString();
                a_textBoxPostcode.Text = a_dataTable.Rows[a_dataGridView.CurrentCell.RowIndex].ItemArray[7].ToString();
                a_textBoxLink.Text = a_dataTable.Rows[a_dataGridView.CurrentCell.RowIndex].ItemArray[8].ToString();
                editingAddress = true;
            }
            catch (Exception) { MessageBox.Show("Selected empty row."); }
        }

        private void a_buttonNew_Click(object sender, EventArgs e)
        {
            a_textBoxTitle.Text = ""; a_textBoxAddress1.Text = ""; a_textBoxAddress2.Text = ""; a_textBoxAddress3.Text = "";
            a_textBoxCity.Text = ""; a_textBoxCounty.Text = ""; a_textBoxPostcode.Text = ""; a_textBoxLink.Text = "";
            editingAddress = false;
        }

        private void a_buttonSave_Click(object sender, EventArgs e) //if existing item is being overwritten
        {
            if (editingAddress == true)
            {
                string query = "UPDATE Addresses SET a_Title=@a_Title, a_Address1=@a_Address1, " +
                    "a_Address2=@a_Address2, a_Address3=@a_Address3, a_CityTown=@a_CityTown, a_CountyState=@a_CountyState, " +
                    "a_Postcode=@a_Postcode, a_Link=@a_Link " +
                    "WHERE AddressID=@AddressID";

                a_command = new OleDbCommand(query, connection);
                a_command.Parameters.AddWithValue("@a_Title", a_textBoxTitle.Text);
                a_command.Parameters.AddWithValue("@a_Address1", a_textBoxAddress1.Text);
                a_command.Parameters.AddWithValue("@a_Address2", a_textBoxAddress2.Text);
                a_command.Parameters.AddWithValue("@a_Address3", a_textBoxAddress3.Text);
                a_command.Parameters.AddWithValue("@a_CityTown", a_textBoxCity.Text);
                a_command.Parameters.AddWithValue("@a_CountyState", a_textBoxCounty.Text);
                a_command.Parameters.AddWithValue("@a_Postcode", a_textBoxPostcode.Text);
                a_command.Parameters.AddWithValue("@a_Link", a_textBoxLink.Text);
                a_command.Parameters.AddWithValue("@a_AddressID", Convert.ToInt32(a_dataTable.Rows[a_dataGridView.
                    CurrentCell.RowIndex]["AddressID"]));
                connection.Open();
                a_command.ExecuteNonQuery();
                connection.Close();

                a_dataTable.Clear();
                GetAddresses();
            }
            else //if the item is new
            {
                string query = "INSERT INTO Addresses (a_Title, a_Address1, a_Address2, a_Address3, a_CityTown, a_CountyState, a_Postcode, a_Link) " +
                    "VALUES (@a_Title, @a_Address1, @a_Address2, @a_Address3, @a_CityTown, @a_CountyState, @a_Postcode, @a_Link)";

                a_command = new OleDbCommand(query, connection);
                a_command.Parameters.AddWithValue("@a_Title", a_textBoxTitle.Text);
                a_command.Parameters.AddWithValue("@a_Address1", a_textBoxAddress1.Text);
                a_command.Parameters.AddWithValue("@a_Address2", a_textBoxAddress2.Text);
                a_command.Parameters.AddWithValue("@a_Address3", a_textBoxAddress3.Text);
                a_command.Parameters.AddWithValue("@a_CityTown", a_textBoxCity.Text);
                a_command.Parameters.AddWithValue("@a_CountyState", a_textBoxCounty.Text);
                a_command.Parameters.AddWithValue("@a_Postcode", a_textBoxPostcode.Text);
                a_command.Parameters.AddWithValue("@a_Link", a_textBoxLink.Text);
                connection.Open();
                a_command.ExecuteNonQuery();
                connection.Close();

                a_dataTable.Clear();
                GetAddresses();
            }

            a_textBoxTitle.Text = ""; a_textBoxAddress1.Text = ""; a_textBoxAddress2.Text = ""; a_textBoxAddress3.Text = "";
            a_textBoxCity.Text = ""; a_textBoxCounty.Text = ""; a_textBoxPostcode.Text = ""; a_textBoxLink.Text = "";
            editingAddress = false;
        }



        // Password Generator
        Random RandomCharacter = new Random();
        int currentPasswordLength = 0;

        private void trackBarPasswordLength_Scroll(object sender, EventArgs e)
        {
            currentPasswordLength = trackBarPasswordLength.Value;
            randomPasswordGenerator(currentPasswordLength);
        }

        private void randomPasswordGenerator(int passwordLength)
        {

            char[] Characters =
            {
                    'Q','W','E','R','T','Y','U','I','O','P','A','S','D','F','G','H','J','K','L','Z',
                    'X','C','V','B','N','M','q','w','e','r','t','y','u','i','o','p','a','s','d','f',
                    'g','h','j','k','l','z','x','c','v','b','n','m','1','2','3','4','5','6','7','8',
                    '9','0','!','£','$','%','^','&','*','(',')','_','-','+','=','{','}','[',']',';',
                    ':','@','#','~',',','<','>','.','/','?'
            };

            string randomPassword = "";

            for (int i = 0; i < passwordLength; i++)
            {
                randomPassword += Characters[RandomCharacter.Next(0, 90)];
            }

            labelPassword.Text = randomPassword;
        }

        private void buttonCopyPassword_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(labelPassword.Text);
        }



        // Hashing Algorithm
        public string toHash256(string passwordInput)
        {
            var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(passwordInput));

            var hashedPassword = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                hashedPassword.Append(bytes[i].ToString("x2"));
            }
            return hashedPassword.ToString();
        }
    }
}