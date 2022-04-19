namespace WinFormExpl
{
    public partial class MainForm_HFXPZZ : Form
    {
        //Tagv�ltoz�k l�trehoz�sa
        FileInfo loadedFile = null; //kor�bban bet�l�tt f�jl
        int counter; //sz�ml�l�
        readonly int counterInitialValue; //sz�ml�l� kezdeti �rt�ke

        public MainForm_HFXPZZ()
        {
            InitializeComponent();
            counterInitialValue = 50; //kezdeti �rt�k meghat�roz�sa, az�rt 50, mert 5 m�sodpercenk�nt t�lt �jra a sz�ml�l�, de a visszasz�ml�l� tizedm�sodpercenk�nt l�p
        }

        private void miOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void miExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void miOpen_Click(object sender, EventArgs e)
        {
            //Az el�z�leg l�trehozoztt inputdialog ablakba be�rt adatok beolvas�sa
            InputDialog dlg = new InputDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                //El�r�si �t kiolvas�as
                string result = dlg.Path;
                // MessageBox.Show(result);
                DirectoryInfo parentDI = new DirectoryInfo(result);
                listView1.Items.Clear(); //Kor�bbi tartalom t�rl�se, ha valaki �jra megnyit az open men�ponttal egy mapp�t, akkor csak az jelenjen meg, az al�z�ek ne 

                try
                {
                    //F�jlok lek�r�se
                    foreach (FileInfo fi in parentDI.GetFiles())
                    {
                        //Csak k�t oszlopot jelen�t�nk meg, de lek�rj�k az el�r�si utat is a k�s�bbi feladatok miatt, �gy gyakorlatilag h�rom oszlop j�n l�tre az ablak bal oldal�n
                        listView1.Items.Add(new ListViewItem(new String[] { fi.Name, fi.Length.ToString(), fi.FullName }));
                    }
                }
                catch { }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lName_Click(object sender, EventArgs e)
        {

        }

        private void listView1_Click(object sender, MouseEventArgs e)
        {
            //Ha valki r�kattint egy f�jl nev�re
            if (listView1.SelectedItems.Count != 1) return;
            //Kiolvassuk a kiv�lasztott f�jl el�r�si �tj�t
            string fullName = listView1.SelectedItems[0].SubItems[2].Text;

            if (fullName != null)
            {
                //Az el�tte l�trehozott k�t adatmez�be be�rjuk az adott f�jl nev�t �s l�trehoz�s�nak ide�pontj�t
                lName.Text = listView1.SelectedItems[0].SubItems[0].Text;
                lCreated.Text = File.GetCreationTime(fullName).ToString();

            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Ha valaki dupl�n kattint egy f�jl nev�re
            if (listView1.SelectedItems.Count != 1) return;
            string fullName = listView1.SelectedItems[0].SubItems[2].Text;

            if (fullName != null)
            {
                //Id�z�t� elind�t�sa, v�ltoz�k be�ll�t�sa
                reloadTimer.Start();
                counter = counterInitialValue;
                loadedFile = new FileInfo(fullName);

                //A f�jl tartalm�nak bem�sol�sa az ablak jobb als� fel�ben l�v� t�bbsoros textpanel-be
                tContent.Text = File.ReadAllText(fullName);

                //A f�ggv�ny t�bbi r�sze tulajdonk�ppen megegyezik az el�z� f�ggv�nnyel
                lName.Text = listView1.SelectedItems[0].SubItems[0].Text;
                lCreated.Text = File.GetCreationTime(fullName).ToString();

            }
        }

        private void reloadTimer_Tick(object sender, EventArgs e)
        {
            counter--;
            //Visszasz�mol�s, tizedm�sodpercenk�nt cs�kken eggyel
            detailsPanel.Invalidate();
            if (counter <= 0)
            {

                //az id�z�t� lej�rtakor a sz�ml�l� vissza�ll�t�sa eredeti �rt�k�re �s a f�jl tartalm�nak �jra beolvas�sa
                counter = counterInitialValue;
                tContent.Text = File.ReadAllText(loadedFile.FullName);
            }
        }

        private void detailsPanel_Paint(object sender, PaintEventArgs e)
        {
            //A panel-be megrajzolja a t�glalapot a megadott param�terekkel, ami azt jelzi, hogy mennyi id� van m�g h�tra a friss�t�s el�tt 
            if (loadedFile != null)
                e.Graphics.FillRectangle(Brushes.Brown, 0, 0, 120 / 50 * counter, 6);
        }
    }
}