using System;
using System.Collections.Generic;
using System.IO;
using EmlakOdevii;

namespace OdevUygulamasi
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("1- Kiralık Ev");
                Console.WriteLine("2- Satılık Ev");
                Console.Write("Seçiminizi yapınız: ");
                string karar = Console.ReadLine();

                switch (karar)
                {
                    case "1":
                        KiralikEvMenu();
                        break;
                    case "2":
                        SatilikEvMenu();
                        break;
                    default:
                        Console.WriteLine("Geçersiz seçim. Lütfen tekrar deneyin.");
                        break;
                }
            }
        }

        static void KiralikEvMenu()
        {
            while (true)
            {
                Console.WriteLine("1- Kayıtlı evleri görüntüle");
                Console.WriteLine("2- Yeni ev girişi");
                Console.Write("Seçiminizi yapınız: ");
                string karar = Console.ReadLine();

                switch (karar)
                {
                    case "1":
                        EvleriGoruntule<KiralikEvlerr>("kiralik_evler.txt");
                        break;
                    case "2":
                        YeniEvGirisi<KiralikEvlerr>("kiralik_evler.txt");
                        break;
                    default:
                        Console.WriteLine("Geçersiz seçim. Lütfen tekrar deneyin.");
                        break;
                }
            }
        }

        static void SatilikEvMenu()
        {
            while (true)
            {
                Console.WriteLine("1- Kayıtlı evleri görüntüle");
                Console.WriteLine("2- Yeni ev girişi");
                Console.Write("Seçiminizi yapınız: ");
                string karar = Console.ReadLine();

                switch (karar)
                {
                    case "1":
                        EvleriGoruntule<SatilikEvlerr>("satilik_evler.txt");
                        break;
                    case "2":
                        YeniEvGirisi<SatilikEvlerr>("satilik_evler.txt");
                        break;
                    default:
                        Console.WriteLine("Geçersiz seçim. Lütfen tekrar deneyin.");
                        break;
                }
            }
        }

        static List<T> DosyadanOku<T>(string dosyaAdi) where T : Ev, new()
        {
            List<T> evler = new List<T>();

            if (File.Exists(dosyaAdi))
            {
                using (StreamReader sr = new StreamReader(dosyaAdi))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        T ev = new T();
                        ev.OdaSayisi = Convert.ToInt32(parts[0]);
                        ev.KatSayisi = Convert.ToInt32(parts[1]);               // !!!!!!!!!!!!!!!!!!!!!!!!
                        ev.Alan = Convert.ToDouble(parts[2]);
                        ev.Semt = parts[3];

                        if (ev is KiralikEvlerr)
                        {
                            KiralikEvlerr kiralikEv = ev as KiralikEvlerr;
                            kiralikEv.Kira = Convert.ToDouble(parts[4]);
                            kiralikEv.Depozito = Convert.ToDouble(parts[5]); //aynı indexi girdiğimde değeri gösteriyor fakat farklı indexte 0 veriyor.
                            evler.Add(ev);
                        }
                        else if (ev is SatilikEvlerr)
                        {
                            SatilikEvlerr satilikEv = ev as SatilikEvlerr;
                            satilikEv.Fiyat = Convert.ToDouble(parts[6]);  // index hatasını sor alanla aynı değeri veriyor.
                            evler.Add(ev);
                        }
                    }
                }
            }

            return evler;
        }


        static void EvleriGoruntule<T>(string dosyaAdi) where T : Ev, new()
        {
            List<T> evler = DosyadanOku<T>(dosyaAdi);

            foreach (var ev in evler)
            {
                Console.WriteLine($"Oda Sayısı: {ev.OdaSayisi}, Kat Sayısı: {ev.KatSayisi}, Alan: {ev.Alan}, Semt: {ev.Semt}");


                if (ev is KiralikEvlerr)
                {
                    KiralikEvlerr kiralikEv = ev as KiralikEvlerr;
                    Console.WriteLine($"Kira: {kiralikEv.Kira}, Depozito: {kiralikEv.Depozito}");
                }

                else if (ev is SatilikEvlerr)
                {
                    SatilikEvlerr satilikEv = ev as SatilikEvlerr;
                    Console.WriteLine($"Fiyat: {satilikEv.Fiyat}");
                }

                Console.WriteLine();
            }
        }

        static void YeniEvGirisi<T>(string dosyaAdi) where T : Ev, new()
        {
            List<T> evler = DosyadanOku<T>(dosyaAdi);

            while (true)
            {
                T yeniEv = new T();

                Console.WriteLine("Ev bilgilerini giriniz:");
                Console.Write("Oda Sayısı: ");
                yeniEv.OdaSayisi = Convert.ToInt32(Console.ReadLine());
                Console.Write("Kat Sayısı: ");
                yeniEv.KatSayisi = Convert.ToInt32(Console.ReadLine());
                Console.Write("Alan: ");
                yeniEv.Alan = Convert.ToDouble(Console.ReadLine());
                Console.WriteLine("Semt: ");
                yeniEv.Semt = Console.ReadLine();



                if (yeniEv is KiralikEvlerr)
                {

                    KiralikEvlerr kiralikEv = new KiralikEvlerr();
                    Console.WriteLine("Kira: ");
                    kiralikEv.Kira = Convert.ToDouble(Console.ReadLine());
                    Console.WriteLine("Depozito: ");
                    kiralikEv.Depozito = Convert.ToDouble(Console.ReadLine());
                    evler.Add(yeniEv);

                }
                else if (yeniEv is SatilikEvlerr)
                {

                    Console.WriteLine("Fiyat: ");
                    SatilikEvlerr satilikEv = new SatilikEvlerr();
                    satilikEv.Fiyat = Convert.ToDouble(Console.ReadLine());
                    evler.Add(yeniEv);

                }

                Console.Write("Tamam mı (t) / Devam mı (d): ");  // main metdouna dönmediği sürece return yada break ile 
                string devam = Console.ReadLine();               // bir önceki seçime gidiyordu. 
                if (devam.ToLower() != "d")
                {
                    DosyayaYaz(evler, dosyaAdi);
                    Console.WriteLine("Dosyaya kaydedildi.");
                    Main(new string[0]);
                    return;
                }

            }


        }

        static void DosyayaYaz<T>(List<T> evler, string dosyaAdi) where T : Ev
        {
            using (StreamWriter sw = new StreamWriter(dosyaAdi))
            {
                foreach (var ev in evler)
                {
                    if (ev is KiralikEvlerr)
                    {
                        KiralikEvlerr kiralikEv = ev as KiralikEvlerr;
                        sw.WriteLine($"{kiralikEv.OdaSayisi},{kiralikEv.KatSayisi},{kiralikEv.Alan},{kiralikEv.Semt},{kiralikEv.Kira},{kiralikEv.Depozito}");
                    }
                    else if (ev is SatilikEvlerr)
                    {
                        SatilikEvlerr satilikEv = ev as SatilikEvlerr;
                        sw.WriteLine($"{satilikEv.OdaSayisi},{satilikEv.KatSayisi},{satilikEv.Alan},{satilikEv.Semt},{satilikEv.Fiyat}");
                    }
                }
            }
        }
    }
}