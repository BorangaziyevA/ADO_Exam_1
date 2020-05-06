using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Security.Principal;

namespace ExamWork_1
{
    class Program
    {
        private static Timer aTimer;
        static Model1 context = new Model1();
        public static void Main(string[] args)
        {

            try
            {
                var x = context.TableExchangeRate.ToList();
                SetTimer();
                Console.ReadLine();
                aTimer.Stop();
                aTimer.Dispose();

            }
            catch (Exception ex)
            {
                try
                {
                    // отправитель - устанавливаем адрес и отображаемое в письме имя
                    MailAddress from = new MailAddress("(Ваш)@gmail.com", "");
                    // кому отправляем
                    MailAddress to = new MailAddress("gersen.e.a@gmail.com");
                    // создаем объект сообщения
                    MailMessage m = new MailMessage(from, to);
                    // тема письма
                    m.Subject = "Тест";
                    // текст письма
                    m.Body = ex.Message + "   |   " + ex.InnerException;
                    // письмо представляет код html
                    m.IsBodyHtml = true;
                    // адрес smtp-сервера и порт, с которого будем отправлять письмо
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                    // логин и пароль
                    smtp.Credentials = new NetworkCredential("(Ваш)@gmail.com", "Ваш пароль");
                    smtp.EnableSsl = true;
                    smtp.Send(m);
                    Console.WriteLine("Письмо об ошибке отправлено");
                    
                }
                catch (Exception ex1)
                {
                    Console.WriteLine(ex1.Message);
                    
                }
            }
        }


        private static void SetTimer()
        {
            aTimer = new System.Timers.Timer(100);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.Enabled = true;
            aTimer.AutoReset = false;
            SetSecondTimer();
        }

        private static void SetSecondTimer()
        {
            aTimer = new System.Timers.Timer(300000);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            XDocument doc = XDocument.Load("http://www.nationalbank.kz/rss/rates.xml");
            List<XElement> x = doc.Descendants("item").ToList();
            if (context.TableExchangeRate.FirstOrDefault() == null)
            {
                Update(x);
            }
            else
            {
                var y = context.TableExchangeRate.ToList();

                foreach (var item in x)
                {
                    if (item.Element("title").Value == y[1].title)
                        if (item.Element("change").Value != y[1].change)
                            Update(x);

                    if (item.Element("title").Value == y[0].title)
                        if (item.Element("change").Value != y[0].change)
                            Update(x);

                    if (item.Element("title").Value == y[2].title)
                        if (item.Element("change").Value != y[2].change)
                            Update(x);
                }
            }
            Console.WriteLine("Для выхода введите что угодно");
        }



        private static void Update(List<XElement> x)
        {
            var exs = from o in context.TableExchangeRate
                      select o;
            foreach (var item in exs)
            {
                context.TableExchangeRate.Remove(item);
            }
            foreach (var items in x)
            {
                TableExchangeRate ex = new TableExchangeRate();
                ex.title = items.Element("title").Value;
                ex.quant = Convert.ToInt32(items.Element("quant").Value);
                ex.description = Convert.ToDouble((items.Element("description").Value).Replace(".", ","));
                ex.change = items.Element("change").Value;
                ex.index = items.Element("index").Value;
                ex.pubDate = Convert.ToDateTime(items.Element("pubDate").Value);
                context.TableExchangeRate.Add(ex);
            }
            context.SaveChanges();
        }
    }
}



