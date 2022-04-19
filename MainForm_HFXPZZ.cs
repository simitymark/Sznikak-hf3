namespace WinFormExpl
{
    public partial class MainForm_HFXPZZ : Form
    {
        //Tagváltozók létrehozása
        FileInfo loadedFile = null; //korábban betölött fájl
        int counter; //számláló
        readonly int counterInitialValue; //számláló kezdeti értéke

        public MainForm_HFXPZZ()
        {
            InitializeComponent();
            counterInitialValue = 50; //kezdeti érték meghatározása, azért 50, mert 5 másodpercenként tölt újra a számláló, de a visszaszámláló tizedmásodpercenként lép
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
            //Az elõzõleg létrehozoztt inputdialog ablakba beírt adatok beolvasása
            InputDialog dlg = new InputDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                //Elérési út kiolvasáas
                string result = dlg.Path;
                // MessageBox.Show(result);
                DirectoryInfo parentDI = new DirectoryInfo(result);
                listView1.Items.Clear(); //Korábbi tartalom törlése, ha valaki újra megnyit az open menüponttal egy mappát, akkor csak az jelenjen meg, az alõzõek ne 

                try
                {
                    //Fájlok lekérése
                    foreach (FileInfo fi in parentDI.GetFiles())
                    {
                        //Csak két oszlopot jelenítünk meg, de lekérjük az elérési utat is a késõbbi feladatok miatt, így gyakorlatilag három oszlop jön létre az ablak bal oldalán
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
            //Ha valki rákattint egy fájl nevére
            if (listView1.SelectedItems.Count != 1) return;
            //Kiolvassuk a kiválasztott fájl elérési útját
            string fullName = listView1.SelectedItems[0].SubItems[2].Text;

            if (fullName != null)
            {
                //Az elõtte létrehozott két adatmezõbe beírjuk az adott fájl nevét és létrehozásának ideõpontját
                lName.Text = listView1.SelectedItems[0].SubItems[0].Text;
                lCreated.Text = File.GetCreationTime(fullName).ToString();

            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Ha valaki duplán kattint egy fájl nevére
            if (listView1.SelectedItems.Count != 1) return;
            string fullName = listView1.SelectedItems[0].SubItems[2].Text;

            if (fullName != null)
            {
                //Idõzítõ elindítása, változók beállítása
                reloadTimer.Start();
                counter = counterInitialValue;
                loadedFile = new FileInfo(fullName);

                //A fájl tartalmának bemásolása az ablak jobb alsó felében lévõ többsoros textpanel-be
                tContent.Text = File.ReadAllText(fullName);

                //A függvény többi része tulajdonképpen megegyezik az elõzõ függvénnyel
                lName.Text = listView1.SelectedItems[0].SubItems[0].Text;
                lCreated.Text = File.GetCreationTime(fullName).ToString();

            }
        }

        private void reloadTimer_Tick(object sender, EventArgs e)
        {
            counter--;
            //Visszaszámolás, tizedmásodpercenként csökken eggyel
            detailsPanel.Invalidate();
            if (counter <= 0)
            {

                //az idõzítõ lejártakor a számláló visszaállítása eredeti értékére és a fájl tartalmának újra beolvasása
                counter = counterInitialValue;
                tContent.Text = File.ReadAllText(loadedFile.FullName);
            }
        }

        private void detailsPanel_Paint(object sender, PaintEventArgs e)
        {
            //A panel-be megrajzolja a táglalapot a megadott paraméterekkel, ami azt jelzi, hogy mennyi idõ van még hátra a frissítés elõtt 
            if (loadedFile != null)
                e.Graphics.FillRectangle(Brushes.Brown, 0, 0, 120 / 50 * counter, 6);
        }
    }
}