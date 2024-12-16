using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using De02.Model1;

namespace De02
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                SanPhamContext context = new SanPhamContext();
                List<LoaiSP> listloaiSPs = context.LoaiSPs.ToList();
                List<Sanpham> listSanPhams = context.Sanphams.ToList();
                FillSanPhamCombobox(listloaiSPs);
                BindGrid(listSanPhams);
                btnLuu.Enabled = false;
                btnKhongLuu.Enabled = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillSanPhamCombobox(List<LoaiSP> listloaiSPs)
        {
            this.cmbLoaiSP.DataSource = listloaiSPs;
            this.cmbLoaiSP.DisplayMember = "TenLoai";
            this.cmbLoaiSP.ValueMember = "MaLoai";
        }

        private void BindGrid(List<Sanpham> listSanPhams)
        {
            dtgSanPham.Rows.Clear();
            foreach (var item in listSanPhams)
            {
                int index = dtgSanPham.Rows.Add();
                dtgSanPham.Rows[index].Cells[0].Value = item.MaSP;
                dtgSanPham.Rows[index].Cells[1].Value = item.TenSP;
                dtgSanPham.Rows[index].Cells[2].Value = item.Ngaynhap;
                dtgSanPham.Rows[index].Cells[3].Value = item.LoaiSP.TenLoai;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                SanPhamContext db = new SanPhamContext();
                List<Sanpham> sanphamlist = db.Sanphams.ToList();
                if (sanphamlist.Any(s => s.MaSP == txtMaSp.Text));
                {
                    MessageBox.Show("Ma san pham da ton tai. Vui long nhap ma khac. ", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                var newSanPham = new Sanpham
                {
                    MaSP = txtMaSp.Text,
                    TenSP = txtTenSP.Text,
                    Ngaynhap = dtpNgayNhap.Value,
                    Maloai = cmbLoaiSP.SelectedValue.ToString(),
                };
                db.Sanphams.Add(newSanPham);
                db.SaveChanges();
                BindGrid(db.Sanphams.ToList());
                MessageBox.Show("Them san pham thanh cong !", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }catch (Exception ex)
            {
                MessageBox.Show("Loi" + ex.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                SanPhamContext db = new SanPhamContext();
                List<Sanpham> sanphams = db.Sanphams.ToList();
                var sanpham = sanphams.FirstOrDefault(s => s.MaSP == txtMaSp.Text);
                if (sanpham != null)
                {
                    if (sanphams.Any(s => s.MaSP == txtMaSp.Text && s.MaSP != sanpham.MaSP))
                    {
                        MessageBox.Show("Ma san pham da ton tai. Vui long nhoa ma khac.", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    sanpham.TenSP = txtTenSP.Text;
                    sanpham.Ngaynhap = dtpNgayNhap.Value;
                    sanpham.Maloai = cmbLoaiSP.SelectedValue.ToString();
                    db.SaveChanges();
                    BindGrid(db.Sanphams.ToList());
                    MessageBox.Show("Chinh sua thong tin san pham thanh cong !", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi" + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                SanPhamContext db = new SanPhamContext();
                List<Sanpham> sanphamlist = db.Sanphams.ToList();
                var sanpham = sanphamlist.FirstOrDefault(s => s.MaSP == txtMaSp.Text);
                if (sanpham != null) {
                    db.Sanphams.Remove(sanpham);
                    db.SaveChanges();
                    BindGrid(db.Sanphams.ToList());
                    MessageBox.Show("San pham da duoc xoa thanh cong! ", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("San pham khong tim thay !", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi" + ex.Message);
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            try
            {
                SanPhamContext db = new SanPhamContext();
                string tensanpham = txtTim.Text.Trim().ToLower();
                var sanphams = db.Sanphams
                                 .Where(s => s.TenSP.ToLower().Contains(tensanpham))
                                 .ToList();
                if (sanphams.Any())
                {
                    BindGrid(sanphams);
                }
                else
                {
                    MessageBox.Show("Khong tim thay ten san pham, vui long nhap lai");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi " + ex.Message);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                SanPhamContext db = new SanPhamContext();
                List<Sanpham> spList = db.Sanphams.ToList();

                var sanpham = spList.FirstOrDefault(s => s.MaSP == txtMaSp.Text);

                if (sanpham != null)
                {
                    sanpham.TenSP = txtTenSP.Text;
                    sanpham.Ngaynhap = dtpNgayNhap.Value;
                    sanpham.Maloai = cmbLoaiSP.SelectedValue.ToString();
                    db.SaveChanges();
                    BindGrid(db.Sanphams.ToList());  
                    MessageBox.Show("Luu thay doi thanh cong", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnLuu.Enabled = false;
                    btnKhongLuu.Enabled = false;
                }
                else
                {
                    MessageBox.Show("San pham khong tim thay!", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi" + ex.Message);
            }
            }

        private void btnKhongLuu_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewRow selectedRow = dtgSanPham.CurrentRow;

                txtMaSp.Text = selectedRow.Cells[0].Value?.ToString() ?? "";
                txtTenSP.Text = selectedRow.Cells[1].Value?.ToString() ?? "";
                dtpNgayNhap.Text = selectedRow.Cells[2].Value?.ToString() ?? "";
                cmbLoaiSP.Text = selectedRow.Cells[3].Value?.ToString() ?? "";
                btnLuu.Enabled = false;
                btnKhongLuu.Enabled = false;

                MessageBox.Show("Huy thay doi thanh cong!", "Thong bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Loi" + ex.Message);
            }
        }

        private void dtgSanPham_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow selectedRow = dtgSanPham.CurrentRow;

            txtMaSp.Text = selectedRow.Cells[0].Value?.ToString() ?? "";
            txtTenSP.Text = selectedRow.Cells[1].Value?.ToString() ?? "";
            dtpNgayNhap.Text = selectedRow.Cells[2].Value?.ToString() ?? "";
            cmbLoaiSP.Text = selectedRow.Cells[3].Value?.ToString() ?? "";

            btnLuu.Enabled = true;
            btnKhongLuu.Enabled = true;
        }
    }
}
