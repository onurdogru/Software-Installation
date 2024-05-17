using EasyModbus;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text.RegularExpressions;
using ROBIN_BEST_TOUCH.Printer;
using System.Printing;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Data.SqlClient;

namespace ROBIN_BEST_TOUCH
{
    public partial class Main : Form
    {
        private Thread saniyeThread = null;
        public AyarForm AyarFrm;
        public Sifre SifreFrm;
        public ProgAyarForm ProgAyarFrm;
        public ProcessForm ProcessFrm;
        public ProgramlamaForm ProgramlamaFrm;

        private IntPtr ShellHwnd;
        private DateTime lastDateTime = DateTime.Now;
        private ModbusClient modbusClientPLC = null;

        const int M0 = 2048;
        const int M1 = 2049;
        const int M2 = 2050;
        const int M3 = 2051;
        const int M4 = 2052;
        const int M5 = 2053;
        const int M6 = 2054;
        const int M7 = 2055;
        const int M8 = 2056;
        const int M9 = 2057;
        const int M10 = 2058;
        const int M11 = 2059;
        const int M12 = 2060;
        const int M13 = 2061;
        const int M14 = 2062;
        const int M15 = 2063;
        const int M16 = 2064;
        const int M17 = 2065;
        const int M18 = 2066;
        const int M19 = 2067;
        const int M20 = 2068;
        const int M21 = 2069;
        const int M22 = 2070;
        const int M23 = 2071;
        const int M24 = 2072;
        const int M25 = 2073;
        const int M26 = 2074;
        const int M27 = 2075;
        const int M28 = 2076;
        const int M29 = 2077;
        const int M30 = 2078;

        const int X0 = 1024;
        const int X1 = 1025;
        const int X2 = 1026;
        const int X3 = 1027;
        const int X4 = 1028;
        const int X5 = 1029;
        const int X6 = 1030;
        const int X7 = 1031;
        const int X10 = 1032;
        const int X11 = 1033;
        const int X12 = 1034;
        const int X13 = 1035;
        const int X21 = 1041;
        const int X23 = 1043;
        const int X25 = 1045;
        const int X27 = 1047;
        const int X31 = 1049;
        const int X33 = 1051;
        const int X20 = 1040;
        const int X22 = 1042;
        const int X24 = 1044;
        const int X26 = 1046;
        const int X30 = 1048;
        const int X32 = 1050;

        const int D4 = 4100;
        const int D6 = 4102;
        const int D8 = 4104;
        const int D10 = 4106;
        const int D12 = 4108;
        const int D14 = 4110;
        const int D16 = 4112;
        const int D18 = 4114;
        const int D20 = 4116;
        const int D30 = 4126;
        const int D40 = 4136;
        const int D400 = 4496;
        const int D405 = 4501;
        const int D410 = 4506;
        const int D415 = 4511;
        const int D440 = 4536;
        const int D445 = 4541;
        const int D450 = 4546;
        const int D455 = 4551;
        const int D460 = 4556;
        const int D465 = 4561;

        const int Y1 = 1281;
        const int Y2 = 1282;
        const int Y3 = 1283;
        const int Y4 = 1284;
        const int Y5 = 1285;
        const int Y6 = 1286;
        const int Y7 = 1287;
        const int Y10 = 1288;
        const int Y11 = 1289;
        const int Y12 = 1290;
        const int Y13 = 1291;
        const int Y20 = 1296;
        const int Y21 = 1297;
        const int Y22 = 1298;
        const int Y23 = 1299;
        const int Y24 = 1300;
        const int Y25 = 1301;
        const int Y40 = 1312;
        const int Y41 = 1313;
        const int Y42 = 1314;
        const int Y43 = 1315;
        const int Y44 = 1316;
        const int Y45 = 1317;
        const int Y46 = 1318;
        const int Y47 = 1319;
        const int Y50 = 1320;
        const int Y51 = 1321;
        const int Y52 = 1322;
        const int Y53 = 1323;
        const int Y35 = 1309;
        const int Y36 = 1310;
        const int Y37 = 1311;
        const int FCT_CARD_NUMBER = 1;
        const int FCT_STEP_MAX = 10;

        //Sıfırlanmamalı
        int totalCard = 0;
        int errorCard = 0;
        public string customMessageBoxTitle = "";
        string logDosyaPath = "";
        byte[] byteArray = new byte[9];
        int byteLenght = 0;
        int DATA1 = 0;
        int DATA2 = 0;
        int DATA3 = 0;
        string FCT_SOFT_VER = "";
        string SOFT_VER = "";

        //Sıfırlanmalı
        int stepState = 0;
        int stepOut = 0;
        int feedbackType = 0;
        int adminTimerCounter = 0;
        int timeoutTimerCounter = 0;
        int saniyeTimerCounter = 0;
        int fctSaniye = 0;
        byte[] arrayRx = new byte[256];
        int counterRxByte = 0;

        //Sıfırlanmalı
        public int yetki = 0;
        public int barcodeCounter = 0;
        string[] filePathTxt = new string[FCT_CARD_NUMBER + 1];
        public string[] barcode50 = new string[FCT_CARD_NUMBER + 1];
        bool[] cardResult = new bool[FCT_CARD_NUMBER + 1];
        public string[] sap_no = new string[FCT_CARD_NUMBER + 1];

        //Ölçümler (Şuan BOŞ'lar)
        double olcum5V = 0;
        double olcum12V = 0;

        //Sabitler (Şuan BOŞ'lar)
        int olcum5VMin = 0;
        int olcum5VMax = 0;
        int olcum12VMin = 0;
        int olcum12VMax = 0;

        SqlConnection SQLConnection;
        bool sqlConnection = false;
        string urun_id = "";
        string urun_barkod = "";
        string son_istasyon_id = "";
        string giris_zamani = "";
        string son_istasyon_zamani = "";
        string urun_durum_no = "";
        string ariza_kodu = "";
        string tamir_edildi = "";
        string son_islem_tamamlandi = "";
        string firma_no = "";
        string urun_kodu = "";
        string panacim_kodu = "";
        string parti_no = "";
        string alan_5 = "";
        string alan_6 = "";
        string alan_7 = "";
        string pcb_barkod = "";

        const string POTA_STATION = "1";
        const string PAKETLEME_STATION = "5";
        const string ICT_STATION_ISTANBUL = "15";
        const string ICT_STATION_BOLU_1 = "19";
        const string ICT_STATION_BOLU_2 = "22";
        const string ALPPLAS_STATION_ROBIN_TB_LONG = "36";      

        const string URUN_DURUM_HURDA = "2";
        const string URUN_DURUM_BEKLETILIYOR = "3";
        const string URUN_DURUM_TAMIR_EDILECEK = "4";
        const string URUN_DURUM_PROCESS = "5";
        const string URUN_DURUM_TAMIR_EDILDI = "6";
        const string URUN_DURUM_HAZIR = "7";
        const string URUN_DURUM_SEVK_EDILECEK = "8";

        const string ARIZA_YOK = "0";
        const string CHECKSUM_HATA = "1";
        const string ARTOUCH_FCT_HATA = "5";
        const string READ_SOFTWARE = "23";
        const string DUO_TESTI = "32";

        public bool traceabilityStatus = false;
        bool btnStateError = false;


        public Main()
        {
            this.AyarFrm = new AyarForm();
            this.AyarFrm.MainFrm = this;
            this.SifreFrm = new Sifre();
            this.SifreFrm.MainFrm = this;
            this.ProgAyarFrm = new ProgAyarForm();
            this.ProgAyarFrm.MainFrm = this;
            this.ProcessFrm = new ProcessForm();
            this.ProcessFrm.MainFrm = this;
            this.ProgramlamaFrm = new ProgramlamaForm();
            this.ProgramlamaFrm.MainFrm = this;
            InitializeComponent();
        }

        public class INIKaydet
        {
            [DllImport("kernel32")]
            private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

            [DllImport("kernel32")]
            private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

            public INIKaydet(string dosyaYolu)
            {
                DOSYAYOLU = dosyaYolu;
            }
            private string DOSYAYOLU = String.Empty;
            public string Varsayilan { get; set; }
            public string Oku(string bolum, string ayaradi)
            {
                Varsayilan = Varsayilan ?? string.Empty;
                StringBuilder StrBuild = new StringBuilder(256);
                GetPrivateProfileString(bolum, ayaradi, Varsayilan, StrBuild, 255, DOSYAYOLU);
                return StrBuild.ToString();
            }
            public long Yaz(string bolum, string ayaradi, string deger)
            {
                return WritePrivateProfileString(bolum, ayaradi, deger, DOSYAYOLU);
            }
        }

        [DllImport("user32.dll")]
        public static extern byte ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string ClassName, string WindowName);

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (traceabilityStatus)
            {
                if (sqlConnection)
                {
                    sqlConnection = false;
                    SQLConnection.Close();
                }
            }
            if (saniyeThread != null)
            {
                saniyeThread.Abort();
            }
            this.serialPort1.Close();
            modbusClientPLC.Disconnect();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.ShellHwnd = Main.FindWindow("Shell TrayWnd", (string)null);
            IntPtr shellHwnd = this.ShellHwnd;
            int num1 = (int)Main.ShowWindow(this.ShellHwnd, 0);
            traceabilityStatus = Ayarlar.Default.chBoxIzlenebilirlik;

            sqlCommonConnection();
            if (sqlConnection)
            {
                settingGetInit();
                FCT_Clear();
                this.ProgramlamaFrm.programlamaInit();
                this.yetkidegistir();
                saniyeThread = new Thread(saniyeThreadFunc);
                saniyeThread.Start();
            }
        }

        private void settingGetInit()
        {
            this.customMessageBoxTitle = Ayarlar.Default.projectName;
            this.projectNameTxt.Text = customMessageBoxTitle;
            this.Text = customMessageBoxTitle;
            this.cardPicture.ImageLocation = Ayarlar.Default.PNGdosyayolu;
            this.logDosyaPath = Ayarlar.Default.txtLogDosya;

            foreach (string portName in SerialPort.GetPortNames())
            {
                this.AyarFrm.SerialPort1Com.Items.Add((object)portName);
            }

            this.serialPort1.PortName = Ayarlar.Default.SerialPort1Com;
            this.serialPort1.BaudRate = Ayarlar.Default.SerialPort1Baud;
            this.serialPort1.DataBits = Ayarlar.Default.SerialPort1dataBits;
            this.serialPort1.StopBits = Ayarlar.Default.SerialPort1stopBit;
            this.serialPort1.Parity = Ayarlar.Default.SerialPort1Parity;
            this.serialPort1.ReceivedBytesThreshold = 1;

            modbusClientPLC = new ModbusClient(Ayarlar.Default.SerialPort2Com);
            modbusClientPLC.UnitIdentifier = 1; //Not necessary since default slaveID = 1;
            modbusClientPLC.Baudrate = Ayarlar.Default.SerialPort2Baud;   // Not necessary since default baudrate = 9600
            modbusClientPLC.Parity = Ayarlar.Default.SerialPort2Parity;
            modbusClientPLC.StopBits = Ayarlar.Default.SerialPort2stopBit;
            modbusClientPLC.ConnectionTimeout = 2000;

            this.timerAdmin.Interval = Ayarlar.Default.timerAdmin;
            this.serialRxTimeout.Interval = Ayarlar.Default.serialRxTimeout;
            /*
            olcum5VMin = Convert.ToInt32(Prog_Ayarlar.Default.v5Min);
            olcum5VMax = Convert.ToInt32(Prog_Ayarlar.Default.v5Max);
            olcum12VMin = Convert.ToInt32(Prog_Ayarlar.Default.v12Min);
            olcum12VMax = Convert.ToInt32(Prog_Ayarlar.Default.v12Max);*/

            if (Ayarlar.Default.chBoxSerial1)  //UDAQ1
            {
                try
                {
                    this.serialPort1.DtrEnable = true;
                    this.serialPort1.Open();
                    lblStatusCom1.Text = "ON";
                    lblStatusCom1.BackColor = Color.Green;
                }
                catch (Exception ex)
                {
                    int num2 = (int)MessageBox.Show("UDAQ1 Port Hatası: " + ex.ToString());
                    lblStatusCom1.Text = "OFF";
                    lblStatusCom1.BackColor = Color.Red;
                }
            }
            if (Ayarlar.Default.chBoxSerial2)  //PLC
            {
                try
                {
                    modbusClientPLC.Connect();
                    lblStatusCom2.Text = "ON";
                    lblStatusCom2.BackColor = Color.Green;
                }
                catch (Exception ex)
                {
                    int num2 = (int)MessageBox.Show("PLC Port Hatası: " + ex.ToString());
                    lblStatusCom2.Text = "OFF";
                    lblStatusCom2.BackColor = Color.Red;
                }
            }
        }

        /****************************************** SQL *************************************************/
        public void sqlCommonConnection()
        {
            if (traceabilityStatus)
            {
                if (sqlConnection == false)
                {
                    try
                    {
                        string connetionString = @"Data Source=192.168.0.8\MEYER;Initial Catalog=Alpplas_Uretim_Takip;User ID=Alpplas_user;Password=Alp-User-21*";
                        SQLConnection = new SqlConnection(connetionString);
                        SQLConnection.Open();
                        ConsoleAppendLine("SQL Baglantısı Açıldı", Color.Green);
                        sqlConnection = true;
                        lblStatusSQL.Text = "ON";
                        lblStatusSQL.BackColor = Color.Green;
                    }
                    catch (Exception ex)
                    {
                        sqlConnection = false;
                        lblStatusSQL.Text = "OFF";
                        lblStatusSQL.BackColor = Color.Red;
                        ConsoleAppendLine("sqlCommonConnection Error: " + ex.Message, Color.Red);
                    }
                }
            }
            else
            {
                lblStatusSQL.Text = "OFF";
                lblStatusSQL.BackColor = Color.Red;
                sqlConnection = true;
            }
        }

        public void sqlWriteError()
        {
            sqlConnection = false;
            lblStatusSQL.Text = "OFF";
            lblStatusSQL.BackColor = Color.Red;
            ConsoleAppendLine("sqlWriteError()", Color.Red);
        }

        public bool urunlerRead(string fullproductCode)
        {
            if (traceabilityStatus)
            {
                sqlCommonConnection();
                if (sqlConnection)
                {
                    try
                    {
                        string sql1 = "SELECT URUN_ID, URUN_BARKOD, SON_ISTASYON_ID, GIRIS_ZAMANI, SON_ISTASYON_ZAMANI, URUN_DURUM_NO, ARIZA_KODU, TAMIR_EDILDI, SON_ISLEM_TAMAMLANDI, FIRMA_NO, URUN_KODU, PANACIM_KODU, PARTI_NO, ALAN_5, ALAN_6, ALAN_7, PCB_BARKOD FROM URUNLER WHERE URUN_BARKOD='" + fullproductCode + "'";
                        SqlCommand command1 = new SqlCommand(sql1, SQLConnection);
                        SqlDataReader dataReader1 = command1.ExecuteReader(CommandBehavior.CloseConnection);

                        bool findState = false;
                        dataReader1.Read();
                        findState = dataReader1.HasRows;
                        if (findState)
                        {
                            urun_id = Convert.ToString(dataReader1.GetValue(0));
                            urun_barkod = Convert.ToString(dataReader1.GetValue(1));
                            son_istasyon_id = Convert.ToString(dataReader1.GetValue(2));
                            giris_zamani = Convert.ToString(dataReader1.GetValue(3));
                            son_istasyon_zamani = Convert.ToString(dataReader1.GetValue(4));
                            urun_durum_no = Convert.ToString(dataReader1.GetValue(5));
                            ariza_kodu = Convert.ToString(dataReader1.GetValue(6));
                            tamir_edildi = Convert.ToString(dataReader1.GetValue(7));
                            son_islem_tamamlandi = Convert.ToString(dataReader1.GetValue(8));
                            firma_no = Convert.ToString(dataReader1.GetValue(9));
                            urun_kodu = Convert.ToString(dataReader1.GetValue(10));
                            panacim_kodu = Convert.ToString(dataReader1.GetValue(11));
                            parti_no = Convert.ToString(dataReader1.GetValue(12));
                            alan_5 = Convert.ToString(dataReader1.GetValue(13));
                            alan_6 = Convert.ToString(dataReader1.GetValue(14));
                            alan_7 = Convert.ToString(dataReader1.GetValue(15));
                            pcb_barkod = Convert.ToString(dataReader1.GetValue(16));
                            ConsoleAppendLine("Ürün Id: " + urun_id, Color.Black);
                            ConsoleAppendLine("Son İstasyon Id: " + son_istasyon_id, Color.Black);
                            ConsoleAppendLine("İlk Giriş Zamanı: " + giris_zamani, Color.Black);
                            ConsoleAppendLine("Son İstasyon Zamanı: " + son_istasyon_zamani, Color.Black);
                            ConsoleAppendLine("Ürün Durum No: " + urun_durum_no, Color.Black);
                            ConsoleAppendLine("Arıza Kodu: " + ariza_kodu, Color.Black);
                            ConsoleAppendLine("Tamir Edildi: " + tamir_edildi, Color.Black);
                            ConsoleAppendLine("Son İşlem Tamamlandı: " + son_islem_tamamlandi, Color.Black);
                            ConsoleNewLine();
                            urunDurum();
                            sonIstasyonDurum();
                            arizaDurum();

                            dataReader1.Close();
                            if (sqlConnection)
                            {
                                sqlConnection = false;
                                SQLConnection.Close();
                            }
                        }
                        else
                        {
                            dataReader1.Close();
                            if (sqlConnection)
                            {
                                sqlConnection = false;
                                SQLConnection.Close();
                            }
                            ConsoleNewLine();
                            ConsoleNewLine();
                            ConsoleAppendLine("YANLIŞ BARKOD YA DA ÜRÜN SİSTEM'DE KAYITLI DEĞİL!", Color.Red);
                            return false;
                        }

                        ConsoleNewLine();
                        ConsoleNewLine();
                        if (son_istasyon_id == POTA_STATION && urun_durum_no == URUN_DURUM_HAZIR && son_islem_tamamlandi == "True")
                        {
                            ConsoleAppendLine("ÜRÜN POTADAN'DAN GEÇMİŞ ICT'YE GİRMELİ", Color.Green);
                            return false;
                        }
                        else if (son_istasyon_id == POTA_STATION && urun_durum_no == URUN_DURUM_TAMIR_EDILDI && son_islem_tamamlandi == "True" && tamir_edildi == "True")
                        {
                            ConsoleAppendLine("ÜRÜN TAMİR'DEN GEÇMİŞ FCT'YE GİREBİLİR", Color.Green);
                            return true;
                        }
                        else if ((son_istasyon_id == ICT_STATION_BOLU_1 || son_istasyon_id == ICT_STATION_BOLU_2 || son_istasyon_id == ICT_STATION_ISTANBUL) && urun_durum_no == URUN_DURUM_HAZIR && son_islem_tamamlandi == "True")
                        {
                            ConsoleAppendLine("ÜRÜN ICT'DEN GEÇMİŞ FCT'YE GİREBİLİR", Color.Green);
                            return true;
                        }
                        else if ((son_istasyon_id == ICT_STATION_BOLU_1 || son_istasyon_id == ICT_STATION_BOLU_2 || son_istasyon_id == ICT_STATION_ISTANBUL) && (urun_durum_no == URUN_DURUM_PROCESS && urun_durum_no == URUN_DURUM_TAMIR_EDILECEK) && son_islem_tamamlandi == "False")
                        {
                            ConsoleAppendLine("ÜRÜN ICT'DEN KALMIŞ FCT'YE GİREMEZ", Color.Red);
                            return false;
                        }
                        else if ((son_istasyon_id == ALPPLAS_STATION_ROBIN_TB_LONG) && urun_durum_no == URUN_DURUM_TAMIR_EDILECEK && son_islem_tamamlandi == "True")
                        {
                            ConsoleAppendLine("KART TAMİRE GİRMELİ YA DA TEKRAR FCT'YE GİREBİLİR", Color.Green);
                            return true;
                        }
                        else if ((son_istasyon_id == ALPPLAS_STATION_ROBIN_TB_LONG) && urun_durum_no == URUN_DURUM_HAZIR && son_islem_tamamlandi == "True")
                        {
                            ConsoleAppendLine("ÜRÜN FCT'DEN DAHA ÖNCE GEÇTİ FCT'YE GİREBİLİR", Color.Green);
                            return true;
                        }
                        else if (son_istasyon_id == PAKETLEME_STATION)
                        {
                            ConsoleAppendLine("KART PAKETLEMEDEN GEÇMİŞ FCT-ICT'YE SOKMAYIN", Color.Orange);
                            return false;
                        }
                        else
                        {
                            ConsoleAppendLine("KART BİR ÖNCEKİ İSTASYONA GİRMELİ", Color.Red);
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        sqlWriteError();
                        ConsoleAppendLine("urunlerRead Error: " + ex.Message, Color.Red);
                        return false;
                    }
                }
                else
                {
                    ConsoleAppendLine("SQL BAĞLANTI KAPALI", Color.Red);
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        private void sonIstasyonDurum()
        {
            if (son_istasyon_id == POTA_STATION)
            {
                if (urun_durum_no == URUN_DURUM_HAZIR)
                {
                    ConsoleAppendLine("SON GİRDİĞİ İSTASYON: POTA", Color.Green);
                }
                else if (urun_durum_no == URUN_DURUM_TAMIR_EDILDI)
                {
                    ConsoleAppendLine("SON GİRDİĞİ İSTASYON: TAMİR", Color.Green);
                }
            }
            else if (son_istasyon_id == PAKETLEME_STATION)
            {
                ConsoleAppendLine("SON GİRDİĞİ İSTASYON: PAKETLEME", Color.Green);
            }
            else if (son_istasyon_id == ICT_STATION_BOLU_1)
            {
                ConsoleAppendLine("SON GİRDİĞİ İSTASYON: ICT-1", Color.Green);
            }
            else if (son_istasyon_id == ICT_STATION_BOLU_2)
            {
                ConsoleAppendLine("SON GİRDİĞİ İSTASYON: ICT-2", Color.Green);
            }
            else if (son_istasyon_id == ICT_STATION_ISTANBUL)
            {
                ConsoleAppendLine("SON GİRDİĞİ İSTASYON: ICT-İSTANBUL", Color.Green);
            }
            else if (son_istasyon_id == ALPPLAS_STATION_ROBIN_TB_LONG)
            {
                ConsoleAppendLine("SON GİRDİĞİ İSTASYON: ALPPLAS_STATION_ROBIN_TB FCT", Color.Green);
            }
        }

        private void urunDurum()
        {
            if (son_istasyon_id == PAKETLEME_STATION)
            {
                ConsoleAppendLine("ÜRÜN DURUM: ÜRÜN SEVKİYATA HAZIR", Color.Green);
            }
            else
            {
                if (urun_durum_no == URUN_DURUM_HURDA)
                {
                    ConsoleAppendLine("ÜRÜN DURUM: ÜRÜN HURDA", Color.Red);
                }
                else if (urun_durum_no == URUN_DURUM_BEKLETILIYOR)
                {
                    ConsoleAppendLine("ÜRÜN DURUM: ÜRÜN BEKLETİLİYOR", Color.Red);
                }
                else if (urun_durum_no == URUN_DURUM_TAMIR_EDILECEK)
                {
                    ConsoleAppendLine("ÜRÜN DURUM: ÜRÜN TAMİR EDİLECEK", Color.Red);
                }
                else if (urun_durum_no == URUN_DURUM_PROCESS)
                {
                    ConsoleAppendLine("ÜRÜN DURUM: ÜRÜN TEST EDİLİYOR VEYA İŞLEM ALTINDA", Color.Green);
                }
                else if (urun_durum_no == URUN_DURUM_TAMIR_EDILDI)
                {
                    ConsoleAppendLine("ÜRÜN DURUM: ÜRÜN TAMİR EDİLDİ", Color.Green);
                }
                else if (urun_durum_no == URUN_DURUM_HAZIR)
                {
                    ConsoleAppendLine("ÜRÜN DURUM: ÜRÜN BİR SONRAKİ TESTE HAZIR", Color.Green);
                }
                else if (urun_durum_no == URUN_DURUM_SEVK_EDILECEK)
                {
                    ConsoleAppendLine("ÜRÜN DURUM: ÜRÜN SEVKİYATA HAZIR", Color.Green);
                }
            }
        }

        private void arizaDurum()
        {
            if (ariza_kodu == ARIZA_YOK)
            {
                ConsoleAppendLine("ARIZA DURUM: ARIZA_YOK", Color.Green);
            }
            else if (ariza_kodu == CHECKSUM_HATA)
            {
                ConsoleAppendLine("ARIZA DURUM: CHECKSUM_HATA", Color.Red);
            }
            else if (ariza_kodu == READ_SOFTWARE)
            {
                ConsoleAppendLine("ARIZA DURUM: YAZILIM_TESTİ_HATA", Color.Red);
            }
            else if (ariza_kodu == DUO_TESTI)
            {
                ConsoleAppendLine("ARIZA DURUM: HABERLEŞME_HATA", Color.Red);
            }
        }

        public string barcodeUnıqTestIdRead(string urun_id)     
        {
            string urun_test_id = "";
            sqlCommonConnection();
            if (sqlConnection)
            {
                try
                {
                    string sql5 = "SELECT URUN_TEST_ID, URUN_ID, MAKINA_NO, TEST_BASLANGIC_ZAMANI, TEST_BITIS_ZAMANI FROM URUN_TESTLER WHERE URUN_ID=" + Convert.ToInt32(urun_id);
                    SqlCommand command5 = new SqlCommand(sql5, SQLConnection);
                    SqlDataReader dataReader5 = command5.ExecuteReader(CommandBehavior.CloseConnection);

                    bool findState = false;
                    dataReader5.Read();
                    findState = dataReader5.HasRows;
                    if (findState)
                    {
                        while (dataReader5.Read())
                        {
                            urun_test_id = Convert.ToString(dataReader5["URUN_TEST_ID"].ToString());
                        }
                        dataReader5.Close();
                        if (sqlConnection)
                        {
                            sqlConnection = false;
                            SQLConnection.Close();
                        }
                        return urun_test_id;
                    }
                    else
                    {
                        dataReader5.Close();
                        if (sqlConnection)
                        {
                            sqlConnection = false;
                            SQLConnection.Close();
                        }
                        ConsoleNewLine();
                        ConsoleNewLine();
                        ConsoleAppendLine("YANLIŞ BARKOD YA DA ÜRÜN SİSTEM'DE KAYITLI DEĞİL !", Color.Red);
                        ConsoleAppendLine("LÜTFEN KARTI POTADAN GEÇİRİN !", Color.Red);
                        return urun_test_id;
                    }
                }
                catch (Exception ex)
                {
                    sqlWriteError();  //ID READ
                    ConsoleAppendLine("barcodeUnıqTestIdRead Error: " + ex.Message, Color.Red);
                    return urun_test_id;
                }
            }
            else
            {
                ConsoleAppendLine("SQL BAĞLANTI KAPALI", Color.Red);
                return urun_test_id;
            }
        }

        private bool urunTestlerInsert()
        {
            if (traceabilityStatus)
            {
                sqlCommonConnection();
                if (sqlConnection)
                {
                    try
                    {
                        DateTime dt = DateTime.Now;
                        string nowYear = Convert.ToString(dt.Year);
                        string nowMonth = Convert.ToString(dt.Month);
                        string nowDay = Convert.ToString(dt.Day);
                        string nowHour = Convert.ToString(dt.Hour);
                        string nowMinute = Convert.ToString(dt.Minute);
                        string nowSecond = Convert.ToString(dt.Second);
                        string mnowSecond = Convert.ToString(dt.Millisecond);
                        string firstTime = nowYear + "-" + nowMonth + "-" + nowDay + " " + nowHour + ":" + nowMinute + ":" + nowSecond + "." + mnowSecond;
                        string sql2 = "INSERT INTO URUN_TESTLER (URUN_ID,MAKINA_NO,TEST_BASLANGIC_ZAMANI,TEST_BITIS_ZAMANI) VALUES('"
                            + urun_id + "'," + "'" + ALPPLAS_STATION_ROBIN_TB_LONG + "'," + "'" + firstTime + "'," + "NULL" + ")";

                        SqlCommand command2 = new SqlCommand(sql2, SQLConnection);
                        SqlDataReader dataReader2 = command2.ExecuteReader();
                        while (dataReader2.Read())
                        {
                            if (command2.ExecuteNonQuery() == 1)
                            {
                                ConsoleAppendLine("SQL Success 1", Color.Green);
                            }
                            else
                            {
                                ConsoleAppendLine("SQL Success 2", Color.Green);
                            }
                        }
                        ConsoleAppendLine("SQL Firt Insert", Color.Green);
                        dataReader2.Close();
                        if (sqlConnection)
                        {
                            sqlConnection = false;
                            SQLConnection.Close();
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        sqlWriteError();
                        ConsoleAppendLine("urunTestlerInsert Error: " + ex.Message, Color.Red);
                        return false;
                    }
                }
                else
                {
                    ConsoleAppendLine("SQL BAĞLANTI KAPALI", Color.Red);
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        private bool urunlerUpdate()
        {
            if (traceabilityStatus)
            {
                sqlCommonConnection();
                if (sqlConnection)
                {
                    try
                    {
                        DateTime dt = DateTime.Now;
                        string nowYear = Convert.ToString(dt.Year);
                        string nowMonth = Convert.ToString(dt.Month);
                        string nowDay = Convert.ToString(dt.Day);
                        string nowHour = Convert.ToString(dt.Hour);
                        string nowMinute = Convert.ToString(dt.Minute);
                        string nowSecond = Convert.ToString(dt.Second);
                        string mnowSecond = Convert.ToString(dt.Millisecond);
                        string lastTime = nowYear + "-" + nowMonth + "-" + nowDay + " " + nowHour + ":" + nowMinute + ":" + nowSecond + "." + mnowSecond;
                        //  string lastTime = "2021-05-03 14:41:10.587";
                        string sql3 = "UPDATE URUNLER SET SON_ISTASYON_ID='" + ALPPLAS_STATION_ROBIN_TB_LONG + "'" + ",URUN_DURUM_NO='" + urun_durum_no + "'"
                            + ",SON_ISLEM_TAMAMLANDI='" + "1" + "'" + ",ARIZA_KODU='" + ariza_kodu + "'"
                            + ",SON_ISTASYON_ZAMANI='" + lastTime + "'" + "WHERE URUN_ID='" + urun_id + "'";
                        SqlCommand command3 = new SqlCommand(sql3, SQLConnection);
                        SqlDataReader dataReader3 = command3.ExecuteReader();
                        while (dataReader3.Read())
                        {
                            if (command3.ExecuteNonQuery() == 1)
                            {
                                ConsoleAppendLine("SQL Success 1", Color.Green);
                            }
                            else
                            {
                                ConsoleAppendLine("SQL Success 2", Color.Green);
                            }
                        }
                        ConsoleAppendLine("SQL Last Update", Color.Green);
                        dataReader3.Close();
                        if (sqlConnection)
                        {
                            sqlConnection = false;
                            SQLConnection.Close();
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        sqlWriteError();
                        ConsoleAppendLine("urunlerUpdate Error: " + ex.Message, Color.Red);
                        return false;
                    }
                }
                else
                {
                    ConsoleAppendLine("SQL BAĞLANTI KAPALI", Color.Red);
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public bool fonksiyonTestInsert(string adimNo, string testTipi, string birim, string altLimit, string ustLimit, string olculen, string sonuc, string bos)
        {
            if (traceabilityStatus)
            {
                string urun_test_id = "";
                urun_test_id = barcodeUnıqTestIdRead(urun_id);
                sqlCommonConnection();
                if (sqlConnection)
                {
                    try
                    {
                        DateTime dt = DateTime.Now;
                        string nowYear = Convert.ToString(dt.Year);
                        string nowMonth = Convert.ToString(dt.Month);
                        string nowDay = Convert.ToString(dt.Day);
                        string nowHour = Convert.ToString(dt.Hour);
                        string nowMinute = Convert.ToString(dt.Minute);
                        string nowSecond = Convert.ToString(dt.Second);
                        string mnowSecond = Convert.ToString(dt.Millisecond);
                        string firstTime = nowYear + "-" + nowMonth + "-" + nowDay + " " + nowHour + ":" + nowMinute + ":" + nowSecond + "." + mnowSecond;
                        string sql4 = "INSERT INTO FONKSIYON_TEST (URUN_TEST_ID,ADIM_NO,TEST_TIPI,BIRIM,ALT_LIMIT,UST_LIMIT,OLCULEN,SONUC,CREATE_DATE) VALUES('"
                          + urun_test_id + "'," + "'" + adimNo + "'," + "'" + testTipi + "'," + "'" + birim + "'," + "'" + altLimit + "'," + "'" + ustLimit + "'," + "'" + olculen + "'," + "'" + sonuc + "'," + "'" + firstTime + "'" + ")";
                        SqlCommand command4 = new SqlCommand(sql4, SQLConnection);
                        SqlDataReader dataReader4 = command4.ExecuteReader();
                        while (dataReader4.Read())
                        {
                            if (command4.ExecuteNonQuery() == 1)
                            {
                                ConsoleAppendLine("SQL Success 1", Color.Green);
                            }
                            else
                            {
                                ConsoleAppendLine("SQL Success 2", Color.Green);
                            }
                        }
                        ConsoleAppendLine("SQL fonksiyonTestInsert Insert", Color.Green);
                        dataReader4.Close();
                        if (sqlConnection)
                        {
                            sqlConnection = false;
                            SQLConnection.Close();
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        sqlWriteError();
                        ConsoleAppendLine("fonksiyonTestInsert Error: " + ex.Message, Color.Red);
                        return false;
                    }
                }
                else
                {
                    ConsoleAppendLine("SQL BAĞLANTI KAPALI", Color.Red);
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public bool fctGenelInsert(string sonuc)
        {
            if (traceabilityStatus)
            {
                string urun_test_id = "";
                urun_test_id = barcodeUnıqTestIdRead(urun_id);
                sqlCommonConnection();
                if (sqlConnection)
                {
                    try
                    {
                        DateTime dt = DateTime.Now;
                        string nowYear = Convert.ToString(dt.Year);
                        string nowMonth = Convert.ToString(dt.Month);
                        string nowDay = Convert.ToString(dt.Day);
                        string nowHour = Convert.ToString(dt.Hour);
                        string nowMinute = Convert.ToString(dt.Minute);
                        string nowSecond = Convert.ToString(dt.Second);
                        string mnowSecond = Convert.ToString(dt.Millisecond);
                        string firstTime = nowYear + "-" + nowMonth + "-" + nowDay + " " + nowHour + ":" + nowMinute + ":" + nowSecond + "." + mnowSecond;
                        string sql6 = "INSERT INTO FCT_GENEL (URUN_ID,URUN_TEST_ID,SONUC,CREATE_DATE,SON_ISTASYON_ID) VALUES('"
                          + urun_id + "'," + "'" + urun_test_id + "'," + "'" + sonuc + "'," + "'" + firstTime + "'," + "'" + ALPPLAS_STATION_ROBIN_TB_LONG + "'" + ")";
                        SqlCommand command6 = new SqlCommand(sql6, SQLConnection);
                        SqlDataReader dataReader6 = command6.ExecuteReader();
                        while (dataReader6.Read())
                        {
                            if (command6.ExecuteNonQuery() == 1)
                            {
                                ConsoleAppendLine("SQL Success 1", Color.Green);
                            }
                            else
                            {
                                ConsoleAppendLine("SQL Success 2", Color.Green);
                            }
                        }
                        ConsoleAppendLine("SQL fctGenelInsert Insert", Color.Green);
                        dataReader6.Close();
                        if (sqlConnection)
                        {
                            sqlConnection = false;
                            SQLConnection.Close();
                        }
                        return true;
                    }
                    catch (Exception ex)
                    {
                        sqlWriteError();
                        ConsoleAppendLine("fctGenelInsert Error: " + ex.Message, Color.Red);
                        return false;
                    }
                }
                else
                {
                    ConsoleAppendLine("SQL BAĞLANTI KAPALI", Color.Red);
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        /****************************************** SERIAL *************************************************/
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            while (serialPort1.BytesToRead > 0)
            {
                arrayRx[counterRxByte] = Convert.ToByte(serialPort1.ReadByte());
                counterRxByte++;
                // Thread.Sleep(100);
            }
            if (counterRxByte == 2)
                this.Invoke(new EventHandler(ShowData1));
        }

        private void ShowData1(object sender, EventArgs e)
        {
            for (int i = 0; i < counterRxByte; i++)
            {
                ConsoleAppendLine("' " + Convert.ToByte(arrayRx[i]) + "' ", Color.Green);
            }
            ConsoleAppendLine("UDAQ1'den geldi.", Color.Green);
            ConsoleNewLine();
            if (feedbackType != 0)
            {
                justFeedbackCheck();
            }
        }

        private void serialWriteByte1()
        {
            try
            {
                for (int i = 0; i < byteLenght; i++)
                {
                    ConsoleAppendLine("' " + Convert.ToByte(byteArray[i]) + " '", Color.Orange);
                }
                ConsoleAppendLine("UDAQ1'e gitti.", Color.Orange);
                serialPort1.Write(byteArray, 0, byteLenght);
            }
            catch (Exception ex)
            {
                ConsoleAppendLine("serialWriteByte1: " + ex.Message, Color.Red);
            }
        }

        private void serialBufferClear()
        {
            for (int i = 0; i <= counterRxByte; i++)
            {
                arrayRx[i] = 0x0;
            }
            counterRxByte = 0;

            if (Ayarlar.Default.chBoxSerial1)
            {
                serialPort1.DiscardInBuffer();
                serialPort1.DiscardOutBuffer();
            }
        }

        /****************************************** MODBUS *************************************************/
        private bool ModBusReadCoils(int address, int length)
        {
            if (modbusClientPLC.Connected)
            {
                try
                {
                    return modbusClientPLC.ReadCoils(address, length)[0];
                }
                catch
                {
                    ConsoleAppendLine("ModBus Read Coil Hatası." + address, Color.Red);
                    return false;
                }
            }
            else
            {
                ConsoleAppendLine("ModBus Kapalı Hatası." + address, Color.Red);
                return false;
            }
        }

        private void ModBusWriteSingleCoils(int address, bool state)
        {
            if (modbusClientPLC.Connected)
            {
                try
                {
                    modbusClientPLC.WriteSingleCoil(address, state);
                }
                catch
                {
                    ConsoleAppendLine("ModBus WriteSingle Coil Hatası." + address, Color.Red);
                }
            }
            else
            {
                ConsoleAppendLine("ModBus Kapalı Hatası." + address, Color.Red);
            }
        }

        private int ModBusReadHoldingRegisters(int address, int length)
        {
            if (modbusClientPLC.Connected)
            {
                try
                {
                    return modbusClientPLC.ReadHoldingRegisters(address, length)[0];
                }
                catch
                {
                    ConsoleAppendLine("ModBus ReadHoldingRegisters Coil Hatası." + address, Color.Red);
                    return 0;
                }
            }
            else
            {
                ConsoleAppendLine("ModBus Kapalı Hatası." + address, Color.Red);
                return 0;
            }
        }

        private bool ModBusReadDiscreteInputs(int address, int length)
        {
            if (modbusClientPLC.Connected)
            {
                try
                {
                    return modbusClientPLC.ReadDiscreteInputs(address, length)[0];
                }
                catch
                {
                    ConsoleAppendLine("ModBus ReadDiscreteInputs Hatası." + address, Color.Red);
                    return false;
                }
            }
            else
            {
                ConsoleAppendLine("ModBus Kapalı Hatası." + address, Color.Red);
                return false;
            }
        }

        /****************************************** INIT *************************************************/
        private void tbBarcodeCurrent_TextChanged(object sender, EventArgs e)
        {
            int maxLenght = 50;

            string barcode = tbBarcodeCurrent.Text;
            if (Convert.ToInt32(barcode.Length) >= maxLenght)
            {
                barcodeCounter++;
                if (ProgramlamaFrm.BarcodeControl(tbBarcodeCurrent.Text))
                {
                    barcode50[barcodeCounter] = tbBarcodeLast.Text;
                    btnFCTInit.BackColor = Color.Green;
                    btnFCTInit.Text = barcodeCounter + ".KART EKLENDİ!";
                }
                else
                {
                    barcodeCounter--;
                    btnFCTInit.BackColor = Color.Red;
                    btnFCTInit.Text = barcodeCounter + ".KART EKLENEMEDİ!";
                }

                if (barcodeCounter == FCT_CARD_NUMBER)
                {
                    btnFCTInit.BackColor = Color.Green;
                    btnFCTInit.Text = "BUTONLARA BASARAK FCT TESTİNİ BAŞLAT";
                    timerInit.Start();
                }
            }
        }

        private void timerInit_Tick(object sender, EventArgs e)
        {
            if (ModBusReadCoils(M0, 1) && ModBusReadDiscreteInputs(X3, 1) && cardFailed == false)  //Güvenlik Biti ve Piston Aşağıda ise
            {
                timerInit.Stop();
                timerInit.Enabled = false;
                lblTimer1.BackColor = Color.Transparent;
                lblTimer1.Text = "OFF";
                timerEmergencyStop.Start();
                lblTimer2.BackColor = Color.Green;
                lblTimer2.Text = "ON";
                FCTInit();
            }
        }
       
        private void timerEmergencyStop_Tick_1(object sender, EventArgs e)
        {
            if (ModBusReadDiscreteInputs(X0, 1) && ModBusReadDiscreteInputs(X3, 1) == false)  //Acil Basıldı ve Piston Yukarıda ise
            {
                timerEmergencyStop.Stop();
                timerEmergencyStop.Enabled = false;
                lblTimer2.BackColor = Color.Transparent;
                lblTimer2.Text = "OFF";
                FCT_Fail();
            }
        }

        private void nextTimer_Tick(object sender, EventArgs e)
        {
            nextTimer.Stop();
            nextTimer.Enabled = false;
            stepState++;
            ProcessFCT();
        }

        private void FCTInit()
        {
            if (serialPort1.IsOpen && modbusClientPLC.Connected)
            {
                saniyeState = true;
                Thread.Sleep(500); 
                for (int i = 1; i <= FCT_CARD_NUMBER; i++)
                {
                    textCreate(i);
                    Thread.Sleep(200);
                }
                serialBufferClear();
                urunTestlerInsert();   //Teste Girdim
                Thread.Sleep(1000);
                btnFCTInit.BackColor = Color.Green;
                btnFCTInit.Text = "TEST BAŞLADI";
                nextTimer.Start();
            }
            else
            {
                CustomMessageBox.ShowMessage("Serial Bağlantısını Kontrol Ediniz!", customMessageBoxTitle, MessageBoxButtons.OK, CustomMessageBoxIcon.Information, Color.Red);
                ProcessFrm.ProcessFailed(1);
            }
        }

        /****************************************************FCT*******************************************************************/
        public void textCreate(int barcodeNum)
        {
            try
            {
                DateTime dt = DateTime.Now;
                string nowYear = Convert.ToString(dt.Year);
                string nowMonth = Convert.ToString(dt.Month);
                string nowDay = Convert.ToString(dt.Day);
                string nowHour = Convert.ToString(dt.Hour);
                string nowMinute = Convert.ToString(dt.Minute);
                string nowSecond = Convert.ToString(dt.Second);
                string name = barcode50[barcodeNum] + "-"+ nowYear + "-" + nowMonth + "-" + nowDay + "-" + nowHour + "-" + nowMinute + "-" + nowSecond;
                filePathTxt[barcodeNum] = logDosyaPath + "//" + name + ".txt"; //
                StreamWriter FileWrite = new StreamWriter(filePathTxt[barcodeNum]);
                FileWrite.Close();
            }
            catch (Exception ex)
            {
                ConsoleAppendLine("textCreate: " + ex.Message, Color.Red);
            }
        }

        public bool cardFailed = false;
        private void ProcessFCT()  
        {
            serialRxTimeout.Start();
            ProcessFrm.ProcessStart(stepState, FCT_STEP_MAX);
            if (stepState == 1) //Adım1 KARTIN PROGRAMLANMASI 
            {
                if (Ayarlar.Default.chBoxProgramlama)
                {
                    logTut1(" ", "", "");
                    logTut1("KARTIN PROGRAMLANMASI", "", "");
                    logTut1(" ", "", "");
                    ModBusWriteSingleCoils(M1, true);     // Programlama Röle Aktif
                    Thread.Sleep(500);

                    if (ProgramlamaFrm.ProgramProduct(sap_no[1]))
                    {
                        logTut1("Programlama Test", ":Passed ", "OK" + " Dönüş");
                        ProcessFrm.ProcessSuccess(stepState);
                    }
                    else
                    {
                        logTut1("Programlama Test", ":Failed ", "NOK" + " Dönüş");
                        ProcessFrm.ProcessFailed(stepState);
                    }
                }
            }
            else if (stepState == 2) //Adım2 TÜM LEDLERİN KONTROL TESTİ
            {
                logTut1(" ", "", "");
                logTut1("TÜM LEDLERİN KONTROL TESTİ ", "", "");
                logTut1(" ", "", "");
                ModBusWriteSingleCoils(M2, true);     // Programlama Röle Pasif
                Thread.Sleep(500);
                ModBusWriteSingleCoils(M3, true);     // Kart Aktif
                Thread.Sleep(1500);
                waitTimer.Start();
                if (CustomMessageBox.ShowMessage("Lütfen 5 sn İçinde BTN1 Basınız ve Tüm LED'lerin Çalıştığını Kontrol Ediniz", customMessageBoxTitle, MessageBoxButtons.YesNo, CustomMessageBoxIcon.Question, Color.Yellow) == DialogResult.Yes)
                {
                    if (btnStateError == false)
                    {
                        logTut1("Tüm LED Kontrol_Test", ":Passed ", " Dönüş");
                        ProcessFrm.ProcessSuccess(stepState);
                    }
                    else
                    {
                        ProcessFrm.ProcessFailed(stepState);
                        logTut1("Tüm LED Kontrol_Test", ":Failed ", " ");
                    }
                }
                else
                {
                    ProcessFrm.ProcessFailed(stepState);
                    logTut1("Tüm LED Kontrol_Test", ":Failed ", " ");
                }
            }
            else if (stepState == 3) //Adım3 FCT HABERLEŞME BAŞLANGIÇ TESTİ
            {
                ModBusWriteSingleCoils(M28, true);     // Kart Pasif
                Thread.Sleep(1000);
                ModBusWriteSingleCoils(M3, true);     // Kart Aktif
                Thread.Sleep(3000);
                logTut1(" ", "", "");
                logTut1("FCT HABERLEŞME BAŞLANGIÇ TESTİ ", "", "");
                logTut1(" ", "", "");
                Thread.Sleep(500);
                byteArray[0] = 0x03;
                byteArray[1] = 0x00;
                byteArray[2] = 0x00;
                byteArray[3] = 0x01;
                byteArray[4] = 0xFB;
                byteLenght = 5;
                serialWriteByte1();  //FeedBack Var
                feedbackType = 1;
                stepOut = 1;
            }
            else if (stepState == 4) //Adım4 FCT YAZILIM VERSİYON KONTROL TESTİ
            {
                logTut1(" ", "", "");
                logTut1("FCT YAZILIM VERSİYON KONTROL TESTİ ", "", "");
                logTut1(" ", "", "");
                Thread.Sleep(500);
                byteArray[0] = 0x06;
                byteArray[1] = 0x00;
                byteArray[2] = 0x03;
                byteArray[3] = 0x00;
                byteArray[4] = 0xF6;
                byteLenght = 5;
                serialWriteByte1();  //FeedBack Var
                feedbackType = 2;
            }
            else if (stepState == 5) //Adım5 YAZILIM VERSİYON KONTROL TESTİ
            {
                logTut1(" ", "", "");
                logTut1("YAZILIM VERSİYON KONTROL TESTİ ", "", "");
                logTut1(" ", "", "");
                Thread.Sleep(500);
                byteArray[0] = 0x06;
                byteArray[1] = 0x00;
                byteArray[2] = 0x06;
                byteArray[3] = 0x00;
                byteArray[4] = 0xF3;
                byteLenght = 5;
                serialWriteByte1();  //FeedBack Var
                feedbackType = 5;
            }
            else if (stepState == 6) //Adım6 VOC I2C KONTROL TESTİ
            {
                logTut1(" ", "", "");
                logTut1("VOC I2C KONTROL TESTİ ", "", "");
                logTut1(" ", "", "");
                Thread.Sleep(500);
                byteArray[0] = 0x06;
                byteArray[1] = 0x00;
                byteArray[2] = 0x1F;
                byteArray[3] = 0x00;
                byteArray[4] = 0xDA;
                byteLenght = 5;
                serialWriteByte1();  //FeedBack Var
                feedbackType = 7;
            }
            else if (stepState == 7) //Adım7 MAINBOARD I2C KONTROL TESTİ
            {
                logTut1(" ", "", "");
                logTut1("MAINBOARD I2C KONTROL TESTİ ", "", "");
                logTut1(" ", "", "");
                Thread.Sleep(500);
                byteArray[0] = 0x06;
                byteArray[1] = 0x00;
                byteArray[2] = 0x20;
                byteArray[3] = 0x00;
                byteArray[4] = 0xD9;
                byteLenght = 5;
                serialWriteByte1();  //FeedBack Var
                feedbackType = 8;
            }
            else if (stepState == 8) //Adım8 NTC SENSÖR KONTROL TESTİ
            {
                ModBusWriteSingleCoils(M4, true);     // NTC Aktif
                logTut1(" ", "", "");
                logTut1("NTC SENSÖR KONTROL TESTİ ", "", "");
                logTut1(" ", "", "");
                Thread.Sleep(500);
                byteArray[0] = 0x06;
                byteArray[1] = 0x00;
                byteArray[2] = 0x21;
                byteArray[3] = 0x00;
                byteArray[4] = 0xD8;
                byteLenght = 5;
                serialWriteByte1();  //FeedBack Var
                feedbackType = 9;
            }
            else if (stepState == 9) //Adım9 BUTTON INIT KONTROL TESTİ
            {
                ModBusWriteSingleCoils(M3, true);     // Kart Aktif
                ModBusWriteSingleCoils(M5, true);     // NTC Pasif
                logTut1(" ", "", "");
                logTut1("BUTTON INIT KONTROL TESTİ ", "", "");
                logTut1(" ", "", "");
                Thread.Sleep(500);
                byteArray[0] = 0x07;
                byteArray[1] = 0x00;
                byteArray[2] = 0x22;
                byteArray[3] = 0xAA;
                byteArray[4] = 0x2C;
                byteLenght = 5;
                serialWriteByte1();  //FeedBack Var
                feedbackType = 10;
            }
            else if (stepState == 10) //Adım10 BUTTON KONTROL TESTİ
            {
                pistonControl();
                logTut1(" ", "", "");
                logTut1("BUTTON KONTROL TESTİ ", "", "");
                logTut1(" ", "", "");
                Thread.Sleep(1000);
                byteArray[0] = 0x06;
                byteArray[1] = 0x00;
                byteArray[2] = 0x23;
                byteArray[3] = 0x00;
                byteArray[4] = 0xD6;
                byteLenght = 5;
                serialWriteByte1();  //FeedBack Var
                feedbackType = 11;
            }

            if ((stepState == 1 || stepState == 2) && cardFailed == false)
                nextTimer.Start();
        }

        private void pistonControl()
        {
            int pistonDelay = Convert.ToInt32(Ayarlar.Default.SerialTx5Timer.ToString());
            ModBusWriteSingleCoils(M6, true);     // Button1 Aktif
            Thread.Sleep(pistonDelay);
            ModBusWriteSingleCoils(M7, true);     // Button1 Aktif
            Thread.Sleep(pistonDelay);
            ModBusWriteSingleCoils(M8, true);     // Button1 Aktif
            Thread.Sleep(pistonDelay);
            ModBusWriteSingleCoils(M9, true);     // Button1 Aktif
            Thread.Sleep(pistonDelay);
            ModBusWriteSingleCoils(M10, true);     // Button1 Aktif
            Thread.Sleep(pistonDelay);
            ModBusWriteSingleCoils(M11, true);     // Button1 Aktif
            Thread.Sleep(pistonDelay);
            ModBusWriteSingleCoils(M12, true);     // Button1 Aktif
            Thread.Sleep(pistonDelay);
            ModBusWriteSingleCoils(M13, true);     // Button1 Aktif
            Thread.Sleep(pistonDelay);
            ModBusWriteSingleCoils(M14, true);     // Button1 Aktif
            Thread.Sleep(pistonDelay);
            ModBusWriteSingleCoils(M15, true);     // Button1 Aktif
            Thread.Sleep(pistonDelay);
            ModBusWriteSingleCoils(M16, true);     // Button1 Aktif
            Thread.Sleep(pistonDelay);
            ModBusWriteSingleCoils(M17, true);     // Button1 Aktif
            Thread.Sleep(pistonDelay);
            ModBusWriteSingleCoils(M18, true);     // Button1 Aktif
            Thread.Sleep(pistonDelay);
            ModBusWriteSingleCoils(M19, true);     // Button1 Aktif
            Thread.Sleep(pistonDelay);
            ModBusWriteSingleCoils(M20, true);     // Button1 Aktif
            Thread.Sleep(pistonDelay);
            ModBusWriteSingleCoils(M21, true);     // Button1 Aktif
            Thread.Sleep(pistonDelay);
            ModBusWriteSingleCoils(M22, true);     // Button1 Aktif
            Thread.Sleep(pistonDelay);
            ModBusWriteSingleCoils(M23, true);     // Button1 Aktif
            Thread.Sleep(pistonDelay);
            ModBusWriteSingleCoils(M24, true);     // Button1 Aktif
            Thread.Sleep(pistonDelay);
            ModBusWriteSingleCoils(M25, true);     // Button1 Aktif
            Thread.Sleep(pistonDelay);
            ModBusWriteSingleCoils(M26, true);     // Button1 Aktif
            Thread.Sleep(pistonDelay);
            ModBusWriteSingleCoils(M27, true);     // Button1 Aktif
        }

        private void justFeedbackCheck()
        {
            serialRxTimeout.Stop();
            serialRxTimeout.Enabled = false;

            if (stepState == 3 && feedbackType == 1 && stepOut == 1)  // FCT_Haberleşme_Başlangıç_Test
            {
                if (arrayRx[0] == 0xAA && arrayRx[1] == 0x55)
                {
                    Thread.Sleep(500);
                    byteArray[0] = 0x03;
                    byteArray[1] = 0x00;
                    byteArray[2] = 0x01;
                    byteArray[3] = 0xFE;
                    byteArray[4] = 0xFD;
                    byteLenght = 5;
                    serialWriteByte1();  //FeedBack Var
                    stepOut = 2;
                }
                else
                {
                    logTut1("FCT_Haberleşme_Başlangıç_Test", ":Failed ", arrayRx[0] + "" + arrayRx[1] + " Dönüş");
                    ProcessFrm.ProcessFailed(stepState);
                }
            }
            else if (stepState == 3 && feedbackType == 1 && stepOut == 2)  // FCT_Haberleşme_Başlangıç_Test
            {
                if (arrayRx[0] == 0xAA && arrayRx[1] == 0x55)
                {
                    Thread.Sleep(500);
                    byteArray[0] = 0x06;
                    byteArray[1] = 0x00;
                    byteArray[2] = 0x1E;
                    byteArray[3] = 0x00;
                    byteArray[4] = 0xDB;
                    byteLenght = 5;
                    serialWriteByte1();  //FeedBack Var
                    stepOut = 3;
                }
                else
                {
                    logTut1("FCT_Haberleşme_Başlangıç_Test", ":Failed ", arrayRx[0] + "" + arrayRx[1] + " Dönüş");
                    ProcessFrm.ProcessFailed(stepState);
                }
            }
            else if (stepState == 3 && feedbackType == 1 && stepOut == 3)  //
            {
                if (arrayRx[0] == 0x03 && arrayRx[1] == 0xFC) 
                {
                    logTut1("FCT_Haberleşme_Başlangıç_Test", ":Passed ", arrayRx[0] + "" + arrayRx[1] + " Dönüş");
                    ProcessFrm.ProcessSuccess(stepState);
                    nextTimer.Start();
                }
                else
                {
                    logTut1("FCT_Haberleşme_Başlangıç_Test", ":Failed ", arrayRx[0] + "" + arrayRx[1] + " Dönüş");
                    ProcessFrm.ProcessFailed(stepState);
                }
            }
            else if (stepState == 4 && feedbackType == 2)  //FCT YAZILIM VERSİYON KONTROL TESTİ
            {
                DATA1 = arrayRx[0];
                Thread.Sleep(500);
                byteArray[0] = 0x06;
                byteArray[1] = 0x00;
                byteArray[2] = 0x04;
                byteArray[3] = 0x00;
                byteArray[4] = 0xF5;
                byteLenght = 5;
                serialWriteByte1();  //FeedBack Var
                feedbackType = 3;
            }
            else if (stepState == 4 && feedbackType == 3)  //
            {
                DATA2 = arrayRx[0];
                Thread.Sleep(500);
                byteArray[0] = 0x06;
                byteArray[1] = 0x00;
                byteArray[2] = 0x05;
                byteArray[3] = 0x00;
                byteArray[4] = 0xF4;
                byteLenght = 5;
                serialWriteByte1();  //FeedBack Var
                feedbackType = 4;
            }
            else if (stepState == 4 && feedbackType == 4)  //
            {
                DATA3 = arrayRx[0];
                FCT_SOFT_VER = Convert.ToString(DATA1) + "." + Convert.ToString(DATA2) + "." + Convert.ToString(DATA3);
                if (Convert.ToString(FCT_SOFT_VER) == Prog_Ayarlar.Default.fctSoftVer)
                {
                    logTut1("FCT_Yazılım_Versiyon_Test", ":Passed ", FCT_SOFT_VER + " Dönüş");
                    ProcessFrm.ProcessSuccess(stepState);
                    nextTimer.Start();
                }
                else
                {
                    logTut1("FCT_Yazılım_Versiyon_Test", ":Failed ", FCT_SOFT_VER + " Dönüş");
                    ProcessFrm.ProcessFailed(stepState);
                }
            }
            else if (stepState == 5 && feedbackType == 5)  //YAZILIM VERSİYON KONTROL TESTİ
            {
                DATA1 = arrayRx[0];
                Thread.Sleep(500);
                byteArray[0] = 0x06;
                byteArray[1] = 0x00;
                byteArray[2] = 0x07;
                byteArray[3] = 0x00;
                byteArray[4] = 0xF2;
                byteLenght = 5;
                serialWriteByte1();  //FeedBack Var
                feedbackType = 6;
            }
            else if (stepState == 5 && feedbackType == 6)  //
            {
                DATA2 = arrayRx[0];
                SOFT_VER = Convert.ToString(DATA1) + "." + Convert.ToString(DATA2);
                if (Convert.ToString(SOFT_VER) == Prog_Ayarlar.Default.softVer)
                {
                    logTut1("Yazılım_Versiyon_Test", ":Passed ", SOFT_VER + " Dönüş");
                    ProcessFrm.ProcessSuccess(stepState);
                    nextTimer.Start();
                }
                else
                {
                    logTut1("Yazılım_Versiyon_Test", ":Failed ", SOFT_VER + " Dönüş");
                    ProcessFrm.ProcessFailed(stepState);
                }
            }
            else if (stepState == 6 && feedbackType == 7)  //VOC_I2C_TEST
            {
                if (arrayRx[0] == 0xAA && arrayRx[1] == 0x55)
                {
                    logTut1("VOC_I2C_Test", ":Passed ", arrayRx[0] + "" + arrayRx[1] + " Dönüş");
                    ProcessFrm.ProcessSuccess(stepState);
                    nextTimer.Start();
                }
                else
                {
                    logTut1("VOC_I2C_Test", ":Failed ", arrayRx[0] + "" + arrayRx[1] + " Dönüş");
                    ProcessFrm.ProcessFailed(stepState);
                }
            }
            else if (stepState == 7 && feedbackType == 8)  //MAINBOARD_I2C_TEST
            {
                if (arrayRx[0] == 0xAA && arrayRx[1] == 0x55)
                {
                    logTut1("MAINBOARD_I2C_Test", ":Passed ", arrayRx[0] + "" + arrayRx[1] + " Dönüş");
                    ProcessFrm.ProcessSuccess(stepState);
                    nextTimer.Start();
                }
                else
                {
                    logTut1("MAINBOARD_I2C_Test", ":Failed ", arrayRx[0] + "" + arrayRx[1] + " Dönüş");
                    ProcessFrm.ProcessFailed(stepState);
                }
            }
            else if (stepState == 8 && feedbackType == 9)  //NTC_SENSÖR_TEST
            {
                if (arrayRx[0] == 0xAA && arrayRx[1] == 0x55)
                {
                    logTut1("NTC_SENSÖR_Test", ":Passed ", arrayRx[0] + "" + arrayRx[1] + " Dönüş");
                    ProcessFrm.ProcessSuccess(stepState);
                    nextTimer.Start();
                }
                else
                {
                    logTut1("NTC_SENSÖR_Test", ":Failed ", arrayRx[0] + "" + arrayRx[1] + " Dönüş");
                    ProcessFrm.ProcessFailed(stepState);
                }
            }
            else if (stepState == 9 && feedbackType == 10)  //BUTTON_INIT_TEST
            {
                if (arrayRx[0] == 0xAA && arrayRx[1] == 0x55)
                {
                    logTut1("BUTTON_INIT_Test", ":Passed ", arrayRx[0] + "" + arrayRx[1] + " Dönüş");
                    ProcessFrm.ProcessSuccess(stepState);
                    nextTimer.Start();
                }
                else
                {
                    logTut1("BUTTON_INIT_Test", ":Failed ", arrayRx[0] + "" + arrayRx[1] + " Dönüş");
                    ProcessFrm.ProcessFailed(stepState);
                }
            }
            else if (stepState == 10 && feedbackType == 11)  //BUTTON_TEST
            {
                if (arrayRx[0] == 0xFF && arrayRx[1] == 0x00)
                {
                    Thread.Sleep(500);
                    byteArray[0] = 0x06;
                    byteArray[1] = 0x00;
                    byteArray[2] = 0x24;
                    byteArray[3] = 0x00;
                    byteArray[4] = 0xD5;
                    byteLenght = 5;
                    serialWriteByte1();  //FeedBack Var
                    feedbackType = 12;
                }
                else
                {
                    logTut1("BUTTON_Test2", ":Failed ", arrayRx[0] + "" + arrayRx[1] + " Dönüş");
                    ProcessFrm.ProcessFailed(stepState);
                }
            }
            else if (stepState == 10 && feedbackType == 12)  ////BUTTON_TEST
            {
                if (arrayRx[0] == 0x07 && arrayRx[1] == 0xF8)
                {
                    logTut1("BUTTON_Test", ":Passed ", arrayRx[0] + "" + arrayRx[1] + " Dönüş");
                    feedbackType = 0;
                    ProcessFrm.ProcessSuccess(stepState);
                    FCT_Success();
                }
                else
                {
                    logTut1("BUTTON_Test2", ":Failed ", arrayRx[0] + "" + arrayRx[1] + " Dönüş");
                    ProcessFrm.ProcessFailed(stepState);
                }
            }

            serialBufferClear();
        }

        private void FCT_Success()
        {
            if (urunlerUpdate())
            {
                udaqEepromSuccessWrite();
                fctGenelInsert("1");
                cardFailed = false;
                FCT_Finish();
                ModBusWriteSingleCoils(M30, true);     // Kartın Enerjisini Kapatıyor, PLC Herşey RESET
                CustomMessageBox.ShowMessage("FCT Testi Sonlandı. Lütfen Tekrar Başlayın!", customMessageBoxTitle, MessageBoxButtons.OK, CustomMessageBoxIcon.Information, Color.Green);
                componentsClear();
                ProcessFrm.Process_Clear();
            }
            else
            {
                ConsoleAppendLine("SQL LAST WRITE PROBLEM", Color.Red);
                FCT_Fail();
            }
        }

        public void FCT_Fail()
        {
            udaqEepromFailWrite();
            fctGenelInsert("0");
            cardFailed = true;
            errorCard = errorCard + FCT_CARD_NUMBER;
            errorCardTxt.Text = Convert.ToString(errorCard);
            FCT_Finish();
            CustomMessageBox.ShowMessage("FCT Testi Başarısız Sonlandı. Lütfen Tekrar Başlayın!", customMessageBoxTitle, MessageBoxButtons.OK, CustomMessageBoxIcon.Information, Color.Red);
            cardFailed = false;
            ModBusWriteSingleCoils(M30, true);     // Kartın Enerjisini Kapatıyor, PLC Herşey RESET
            componentsClear();
            ProcessFrm.Process_Clear();
        }

        public void udaqEepromSuccessWrite()
        {
            Thread.Sleep(500);
            byteArray[0] = 0x07;
            byteArray[1] = 0x00;
            byteArray[2] = 0x00;
            byteArray[3] = 0x03;
            byteArray[4] = 0xF5;
            byteLenght = 5;
            serialWriteByte1();  //FeedBack Var ama Yok
            Thread.Sleep(500);
            byteArray[0] = 0x07;
            byteArray[1] = 0x00;
            byteArray[2] = 0x01;
            byteArray[3] = 0xFC;
            byteArray[4] = 0xFB;   
            byteLenght = 5;
            serialWriteByte1();  //FeedBack Var ama Yok
            Thread.Sleep(2000);
        }

        public void udaqEepromFailWrite()
        {
            Thread.Sleep(500);
            byteArray[0] = 0x07;
            byteArray[1] = 0x00;
            byteArray[2] = 0x00;
            byteArray[3] = 0x02;
            byteArray[4] = 0xF6;
            byteLenght = 5;
            serialWriteByte1();  //FeedBack Var ama Yok
            Thread.Sleep(500);
            byteArray[0] = 0x07;
            byteArray[1] = 0x00;
            byteArray[2] = 0x01;
            byteArray[3] = 0xFD;
            byteArray[4] = 0xFA;   
            byteLenght = 5;
            serialWriteByte1();  //FeedBack Var ama Yok
            Thread.Sleep(2000);
        }

        public void FCT_Finish()
        {
            FCT_Clear();
            Verim();
        }

        private void FCT_Clear()
        {
            timersClear();
            variablesClear();
        }

        private void timersClear()
        {
            nextTimer.Stop();
            nextTimer.Enabled = false;
            timerAdmin.Stop();
            timerAdmin.Enabled = false;
            serialRxTimeout.Stop();
            serialRxTimeout.Enabled = false;
            timerEmergencyStop.Stop();
            timerEmergencyStop.Enabled = false;
            lblTimer2.BackColor = Color.Transparent;
            lblTimer2.Text = "OFF";
            timerInit.Start();
            lblTimer1.BackColor = Color.Green;
            lblTimer1.Text = "ON";
        }

        private void variablesClear()
        {
            btnStateError = false;
            saniyeState = false;
            stepState = 0;
            stepOut = 0;
            feedbackType = 0;
            adminTimerCounter = 0;
            timeoutTimerCounter = 0;
            saniyeTimerCounter = 0;
            fctSaniye = 0;

            yetki = 0;
            barcodeCounter = 0;
            for (int i = 1; i <= FCT_CARD_NUMBER; i++)
            {
                filePathTxt[i] = "";
                barcode50[i] = "";
                cardResult[i] = true;
                sap_no[i] = "";
            }
        }

        private void componentsClear()
        {
            btnFCTInit.BackColor = Color.Yellow;
            btnFCTInit.Text = "KART EKLEMEYE BAŞLAYABİLİRSİNİZ.";
            progressBarFCT.Value = 0;
        }

        private void Verim()
        {
            totalCard = totalCard + FCT_CARD_NUMBER;
            totalCardTxt.Text = Convert.ToString(totalCard);
            verimTxt.Text = Convert.ToString(100 - ((float)((float)errorCard / totalCard)) * 100);
        }

        /****************************************************LOG*******************************************************************/
        private void logTut1(string testName, string testResult, string testState)
        {
            try
            {
                if (logDosyaPath != "")
                {
                    List<string> lines = new List<string>();
                    lines = File.ReadAllLines(filePathTxt[1]).ToList();
                    lines.Add(testName + testResult + testState);
                    ConsoleAppendLine(testName + testResult + testState + "Eklendi", Color.Green);
                    File.WriteAllLines(filePathTxt[1], lines);
                }
                else
                {
                    ConsoleAppendLine("Dosya Yolu Boş Kalamaz", Color.Red);
                }
            }
            catch (Exception ex)
            {
                ConsoleAppendLine("sqlTextYaz: " + ex.Message, Color.Red);
            }
        }

        /****************************************************CONSOLE TEXT*******************************************************************/
        private void rtbConsole_TextChanged(object sender, EventArgs e)
        {
            RichTextBox rtb = sender as RichTextBox;
            rtb.SelectionStart = rtb.Text.Length;
            rtb.ScrollToCaret();
        }

        /*Kullanıcı Arayüzüne Yazı Yazılır*/
        public void ConsoleAppendLine(string text, Color color)
        {
            if (rtbConsole.InvokeRequired)
            {
                rtbConsole.Invoke(new Action(delegate ()
                {
                    rtbConsole.Select(rtbConsole.TextLength, 0);
                    rtbConsole.SelectionColor = color;
                    rtbConsole.AppendText(text + Environment.NewLine);
                    rtbConsole.Select(rtbConsole.TextLength, 0);
                    rtbConsole.SelectionColor = Color.White;
                }));
            }
            else
            {
                rtbConsole.Select(rtbConsole.TextLength, 0);
                rtbConsole.SelectionColor = color;
                rtbConsole.AppendText(text + Environment.NewLine);
                rtbConsole.Select(rtbConsole.TextLength, 0);
                rtbConsole.SelectionColor = Color.White;
            }
        }

        /*Kullanıcı Arayüzünde Bir Satır Boşluk Bırakılır*/
        public void ConsoleNewLine()
        {
            if (rtbConsole.InvokeRequired)
            {
                rtbConsole.Invoke(new Action(delegate ()
                {
                    rtbConsole.AppendText(Environment.NewLine);
                }));
            }
            else
            {
                rtbConsole.AppendText(Environment.NewLine);
            }
        }

        public void ConsoleClean()
        {
            if (rtbConsole.InvokeRequired)
            {
                rtbConsole.Invoke(new Action(delegate ()
                {
                    rtbConsole.Text = "";
                    rtbConsole.Select(rtbConsole.TextLength, 0);
                    rtbConsole.SelectionColor = Color.White;
                }));
            }
            else
            {
                rtbConsole.Text = "";
                rtbConsole.Select(rtbConsole.TextLength, 0);
                rtbConsole.SelectionColor = Color.White;
            }
        }

        /****************************************************PAGE CHANGE*******************************************************************/
        private void btnCikis_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAyar_Click(object sender, EventArgs e)
        {
            int num = (int)this.AyarFrm.ShowDialog();
        }

        private void btnProgAyar_Click(object sender, EventArgs e)
        {
            int num = (int)this.ProgAyarFrm.ShowDialog();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) 
        {
            if (keyData != Keys.L)
                return false;
            if (this.yetki != 0)
            {
                timerAdmin.Stop();
                this.yetki = 0;
                this.yetkidegistir();
            }
            else
            {
                try { int num = (int)this.SifreFrm.ShowDialog(); }
                catch (Exception) { }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void yetkidegistir()
        {
            if (this.yetki == 0)
            {
                this.btnCikis.Enabled = false;
                this.btnAyar.Enabled = false;
                this.btnProgAyar.Enabled = false;

                this.btnCikis.BackColor = Color.Beige;
                this.btnAyar.BackColor = Color.Beige;
                this.btnProgAyar.BackColor = Color.Beige;
            }
            if (this.yetki == 1)
            {
                this.btnCikis.Enabled = true;
                this.btnAyar.Enabled = true;
                this.btnProgAyar.Enabled = true;

                this.btnCikis.BackColor = Color.Red;
                this.btnAyar.BackColor = Color.Red;
                this.btnProgAyar.BackColor = Color.Red;
                timerAdmin.Start();
            }
            if (this.yetki == 2)
            {
                this.btnCikis.Enabled = true;
                this.btnCikis.BackColor = Color.Red;
                this.btnAyar.BackColor = Color.Beige;
                this.btnProgAyar.BackColor = Color.Beige;
                timerAdmin.Start();
            }
        }

        /****************************************************TIMERS*******************************************************************/
        private void timerAdmin_Tick_1(object sender, EventArgs e)
        {
            adminTimerCounter++;
            if (adminTimerCounter == 1)
            {
                adminTimerCounter = 0;
                timerAdmin.Stop();
                this.yetki = 0;
                this.yetkidegistir();
            }
        }

        private void serialRxTimeout_Tick(object sender, EventArgs e)
        {
            timeoutTimerCounter++;
            if (timeoutTimerCounter == 1)
            {
                ConsoleAppendLine("TIMEOUT_RX", Color.Red);
                timeoutTimerCounter = 0;
                serialRxTimeout.Stop();
                serialRxTimeout.Enabled = false;
                ProcessFrm.ProcessFailed(stepState);
            }
        }

        private void saniyeTimer_Tick(object sender, EventArgs e)
        {
            saniyeTimerCounter++;
            if (saniyeTimerCounter == 1)
            {
                saniyeTimerCounter = 0;
                fctTimerTxt.Text = Convert.ToString(++fctSaniye);
            }
        }

        bool saniyeState = false;
        int second = 0;
        int oldSecond = 0;
        private void saniyeThreadFunc()
        {
            for (; ; )
            {
                if (saniyeState)
                {
                    DateTime dt = DateTime.Now;
                    second = dt.Second;
                    if (second != oldSecond)
                    {
                        oldSecond = second;
                        //fctTimerTxt.Text = Convert.ToString(++fctSaniye);  // Teslimde Aç ???
                    }
                    Thread.Sleep(1);
                }
            }
        }

        private void waitTimer_Tick(object sender, EventArgs e)
        {
            btnStateError = true;
            waitTimer.Stop();
            waitTimer.Enabled = false;
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
    }
}

