using Driver;
using Microsoft.VisualBasic;
using System.Text.Json;
using System.Windows.Forms;

namespace App;

public partial class Form1 : Form
{
    private readonly Dictionary<int, KeyHandler> handlers = [];

    public Form1()
    {
        var ghk = new KeyHandler(Keys.Enter, this, 0x01 | 0x02);
        ghk.Register();
        handlers[ghk.GetHashCode()] = ghk;
        InitializeComponent();
        Load += OnLoad;
    }

    private void OnLoad(object? sender, EventArgs e)
    {
        dataGridView1.CellValueChanged -= CellChanged;
        Console.WriteLine("Keyboard Connected {0}", KeyboardManager.Instance.IsConnected());
        ProfileManager.Instance.DiscoverProfiles();
        dataGridView1.Rows.Clear();
        dataGridView1.Rows.Add(ProfileManager.Instance.Profiles.Count);
        for (int i = 0; i < ProfileManager.Instance.Profiles.Count; i++)
        {
            var profile = ProfileManager.Instance.Profiles[i];
            dataGridView1.Rows[i].Cells[0].Value = profile.Name;
            dataGridView1.Rows[i].Cells[1] = new DataGridViewCheckBoxCell();
            dataGridView1.Rows[i].Cells[1].Value = profile.SelectedForQuickSwitch;
        }
        dataGridView1.CellValueChanged += CellChanged;
    }

    private void ButtonClicked(object sender, EventArgs e)
    {
        if (openFileDialog1.ShowDialog(this) is DialogResult.OK)
        {
            var path = openFileDialog1.FileName;
            ProfileManager.Instance.ImportProfile(path);
        }
    }

    private void CellChanged(object? sender, DataGridViewCellEventArgs e)
    {
        var value = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
        ProfileManager.Instance.ProfileItemChanged(e.RowIndex, e.ColumnIndex, value);
    }

    private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {

    }

    private void HandleGlobalKeypress(nint key)
    {
        handlers[(int)key].Callback.Invoke();
    }

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == 0x0312)
        {
            HandleGlobalKeypress(m.WParam);
        }
        base.WndProc(ref m);
    }
}
