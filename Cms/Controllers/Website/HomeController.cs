using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using Cms.Controllers.Website;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using MySql.Data.MySqlClient;

namespace Cms.Controllers
{
    public class HomeController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();


        public ActionResult Index()
        {
            ViewBag.CountVisit = 10000; /*_context.WebsiteView.Count();*/
            ViewBag.CountRegCourse = 10000; /*_context.CoursePayment.Count() + _context.AccountOrderProduct.SearchCount(typeId: 2, status: DataLayer.Enumarables.Enum_Code.ORDER_STATUS_SUCCESS);*/
            ViewBag.CountQuiz = 10000; /*_context.QuizPayment.Where(s => s.Status == 1).ToList().Count() + _context.AccountOrderProduct.SearchCount(typeId: 1001, status: DataLayer.Enumarables.Enum_Code.ORDER_STATUS_SUCCESS);*/
            return PartialView(BaseController.GetView(this));
        }
        public ActionResult Test()
        {
            var list = _context.Product.Where(s => s.ProductTypeId == 1001).ToList();
            foreach (var item in list)
            {
                if (item.ProductCustomValue.Any())
                {
                    var custom = item.ProductCustomValue.FirstOrDefault(s => s.FieldId == 1009);
                    if (custom.ItemId == 1004)
                    {
                        item.ProductCategoryId = 1003;
                    }
                    else if (custom.ItemId == 1005)
                    {
                        item.ProductCategoryId = 1004;
                    }
                    else if (custom.ItemId == 1006)
                    {
                        item.ProductCategoryId = 1005;
                    }
                }
                _context.Product.Update(item);
                _context.Save();
            }
            return PartialView(BaseController.GetView(this));
        }
        public void AddBook()
        {
            string connString = "DataSource=127.0.0.1;Database=new_schema2;UserID=root;Password=165139420;PORT=3306;logging=True;allow batch=true";
            MySqlConnection conn = new MySqlConnection(connString);
            MySqlCommand myCommand = new MySqlCommand("SELECT * FROM new_schema2.order_items;");
            myCommand.Connection = conn;
            conn.Open();
            MySqlDataReader reader;
            reader = myCommand.ExecuteReader();

            var list = new List<BookPayment>();
            while (reader.Read())
            {
                var id = Convert.ToInt32(reader["order_id"]);
                //MySqlCommand myCommand2 = new MySqlCommand("SELECT * FROM new_schema2.order_items where order_id=" + id + ";");
                //myCommand2.Connection = conn;
                //MySqlDataReader reader2;
                //reader2 = myCommand2.ExecuteReader();
                var tem = _context.BookPayment.FirstOrDefault(s => s.OrderId == id);
                if (tem == null)
                {
                    var model = new BookPayment()
                    {
                        OrderId = Convert.ToInt32(reader["order_id"]),
                        //UserId = Convert.ToInt32(reader["user_id"]),
                        Price = Convert.ToDouble(reader["price"]),
                        Count = Convert.ToInt32(reader["qty"]),
                        BookId = Convert.ToInt32(reader["product_id"]) + 122,
                        //Address = Convert.ToString(reader["address"]),

                    };
                    list.Add(model);
                }


                //list.Add(model);
            }
            myCommand.Connection.Close();
            foreach (var item in list)
            {

                _context.BookPayment.Insert(item);
                _context.Save();


            }
        }
        public void AddUser()
        {
            string connString = "DataSource=127.0.0.1;Database=new_schema2;UserID=root;Password=165139420;PORT=3306;logging=True;allow batch=true";
            MySqlConnection conn = new MySqlConnection(connString);
            MySqlCommand myCommand = new MySqlCommand("SELECT * FROM new_schema2.users where id>14036;");
            myCommand.Connection = conn;
            conn.Open();
            MySqlDataReader reader;
            reader = myCommand.ExecuteReader();

            var list = new List<Account>();
            while (reader.Read())
            {

                var id = Convert.ToInt32(reader["id"]);
                if (id != null)
                {
                    if (id > 14035)
                    {
                        id = id + 3000;
                    }
                    //MySqlCommand myCommand2 = new MySqlCommand("SELECT * FROM new_schema2.order_items where order_id=" + id + ";");
                    //myCommand2.Connection = conn;
                    //MySqlDataReader reader2;
                    //reader2 = myCommand2.ExecuteReader();
                    var temp = _context.Account.FirstOrDefault(s => s.ID == id);
                    if (temp == null)
                    {
                        //DateTime? BirthDay = new DateTime();
                        //if (reader["birthdate"] != null)
                        //{
                        //    BirthDay = Convert.ToDateTime(reader["birthdate"]);
                        //}
                        var model = new Account()
                        {
                            ID = id,
                            //UserId = Convert.ToInt32(reader["user_id"]),
                            FullName = Convert.ToString(reader["name"]) + " " + Convert.ToString(reader["surname"]),
                            Email = Convert.ToString(reader["email"]),
                            Mobile = Convert.ToString(reader["mobile"]),
                            Password = "1",
                            Password2 = Convert.ToString(reader["password"]),
                            NationalCode = Convert.ToString(reader["nat_code"]),
                            Address = Convert.ToString(reader["address"]),
                            Company = Convert.ToString(reader["gender"]),
                            UniqueId = null,
                            //BirthDay = null,
                            IsMale = true,
                            ChangeUserName = false,
                            FullNameEn = Convert.ToString(reader["en_name"]) + " " + Convert.ToString(reader["en_surname"]),
                            FatherName = Convert.ToString(reader["father_name"]),
                            FatherNameEn = Convert.ToString(reader["en_father_name"]),
                            Image = Convert.ToString(reader["image"]),
                            ImageNational = Convert.ToString(reader["nat_card_image"]),
                            ImageEduaction = Convert.ToString(reader["education_image"]),
                            ImagePassport = Convert.ToString(reader["passport_image"]),
                            ImageShenasnameh = Convert.ToString(reader["shenasnameh_image"]),
                            ImageInsurance = Convert.ToString(reader["insurance_image"]),
                            ImageCoaching2 = Convert.ToString(reader["coaching2_image"]),
                            ImageCoaching3 = Convert.ToString(reader["coaching3_image"]),
                            ZipCode = Convert.ToString(reader["zipcode"]),


                        };
                        list.Add(model);
                    }
                }


                //list.Add(model);
            }
            myCommand.Connection.Close();
            foreach (var item in list)
            {

                _context.Account.Insert(item);
                _context.Save();


            }
        }

        public void UpdateBook()
        {
            string connString = "DataSource=127.0.0.1;Database=new_schema2;UserID=root;Password=165139420;PORT=3306;logging=True;allow batch=true";
            MySqlConnection conn = new MySqlConnection(connString);
            MySqlCommand myCommand = new MySqlCommand("SELECT * FROM new_schema2.orders;");
            myCommand.Connection = conn;
            conn.Open();
            MySqlDataReader reader;
            reader = myCommand.ExecuteReader();

            var list = new List<BookPayment>();
            while (reader.Read())
            {
                var id = Convert.ToInt32(reader["id"]);
                var orders = _context.BookPayment.Where(s => s.OrderId == id);
                foreach (var item in orders)
                {
                    var userid = Convert.ToInt32(reader["user_id"]);
                    if (userid > 14035)
                    {
                        userid = userid + 3000;
                    }
                    item.Address = Convert.ToString(reader["address"]);
                    item.UserId = userid;
                    _context.BookPayment.Update(item);

                }

            }
            _context.Save();
            myCommand.Connection.Close();

        }

        public void AddQuiz()
        {
            string connString = "DataSource=127.0.0.1;Database=new_schema2;UserID=root;Password=165139420;PORT=3306;logging=True;allow batch=true";
            MySqlConnection conn = new MySqlConnection(connString);
            MySqlCommand myCommand = new MySqlCommand("SELECT * FROM new_schema2.quiz_payments");
            myCommand.Connection = conn;
            conn.Open();
            MySqlDataReader reader;
            reader = myCommand.ExecuteReader();

            var list = new List<QuizPayment>();
            while (reader.Read())
            {
                var ids = reader["quiz_ids"].ToString().Split(',');
                int idpay = Convert.ToInt32(reader["id"]);
                var temp = _context.QuizPayment.FirstOrDefault(s => s.PaymentId == idpay);
                if (temp == null)
                {
                    foreach (var item in ids)
                    {
                        var value = item.Replace("[", "").Replace("]", "");
                        int id = int.Parse(value);
                        var userid = Convert.ToInt32(reader["user_id"]);
                        if (userid > 14035)
                        {
                            userid = userid + 3000;
                        }
                        var model = new QuizPayment()
                        {
                            PaymentId = Convert.ToInt32(reader["id"]),
                            UserId = userid,
                            Price = Convert.ToDouble(reader["total_price"]),
                            QuizId = id + 82,
                            Status = Convert.ToByte(reader["paid_status"])

                        };
                        list.Add(model);
                    }

                }



                //list.Add(model);
            }
            myCommand.Connection.Close();
            foreach (var item in list)
            {
                var model = _context.QuizPayment.FirstOrDefault(s => s.QuizId == item.QuizId && s.UserId == item.UserId && s.Status == item.Status);
                if (model == null)
                {
                    _context.QuizPayment.Insert(item);
                    _context.Save();
                }

            }
        }
        public void AddCourse()
        {
            try
            {
                //This is my connection string i have assigned the database file address path
                string connString = "DataSource=127.0.0.1;Database=new_schema2;UserID=root;Password=165139420;PORT=3306;logging=True;allow batch=true";
                MySqlConnection conn = new MySqlConnection(connString);
                MySqlCommand myCommand = new MySqlCommand("SELECT * FROM new_schema2.course_installments");
                myCommand.Connection = conn;
                conn.Open();
                MySqlDataReader reader;
                reader = myCommand.ExecuteReader();

                var list = new List<CoursePayment>();
                while (reader.Read())
                {
                    int courseId = Convert.ToInt32(reader["course_id"]);
                    var userid = Convert.ToInt32(reader["user_id"]);
                    if (userid > 14035)
                    {
                        userid = userid + 3000;
                    }
                    if (courseId > 79)
                    {
                        courseId = courseId + 971;
                    }
                    var model = new CoursePayment()
                    {
                        ID = Convert.ToInt32(reader["id"]),
                        UserId = userid,
                        CourseId = courseId,
                        Amount = Convert.ToDouble(reader["amount"]),
                        //CreatedAt = Convert.ToDateTime(reader["created_at"]),
                        //UpdatedAt = Convert.ToDateTime(reader["updated_at"]),
                        PaidStatus = Convert.ToByte(reader["paid_status"]),

                    };


                    list.Add(model);
                }
                myCommand.Connection.Close();
                foreach (var item in list)
                {
                    _context.CoursePayment.Insert(item);
                    _context.Save();
                }


            }
            catch (Exception ex)
            {

            }
        }
        public void AddProduct()
        {
            try
            {
                //This is my connection string i have assigned the database file address path
                string connString = "DataSource=127.0.0.1;Database=new_schema2;UserID=root;Password=165139420;PORT=3306;logging=True;allow batch=true";
                MySqlConnection conn = new MySqlConnection(connString);
                MySqlCommand myCommand = new MySqlCommand("SELECT * FROM new_schema2.courses where id>79");
                myCommand.Connection = conn;
                conn.Open();
                MySqlDataReader reader;
                reader = myCommand.ExecuteReader();

                var list = new List<Product>();
                while (reader.Read())
                {
                    //int courseId = Convert.ToInt32(reader["course_id"]);

                    var model = new Product()
                    {
                        Name = reader["title"].ToString(),
                        H1 = reader["title"].ToString(),
                        Summary = reader["title"].ToString(),
                        Title = reader["title"].ToString(),
                        NoFollow = false,
                        NoIndex = false,
                        ProductTypeId = 2,
                        ShowHomePage = false,
                        Description = reader["description"].ToString(),
                        Active = true,
                        IsService = true,
                        StatusId = 29,
                        CreateDateTime = DateTime.Now,
                        StartDatetime = Convert.ToDateTime(reader["enroll_start_at"]),
                        EndDatetime = Convert.ToDateTime(reader["enroll_end_at"]),
                        FromDatetime = Convert.ToDateTime(reader["start_at"]),
                        ToDatetime = Convert.ToDateTime(reader["end_at"]),
                        LinkTelegram = reader["telegram_link"].ToString(),
                        Image = reader["image"].ToString()
                    };
                    list.Add(model);
                }
                myCommand.Connection.Close();
                foreach (var item in list)
                {
                    _context.Product.Insert(item);
                    _context.Save();
                }


            }
            catch (Exception ex)
            {

            }
        }

        public void AddCourse2()
        {
            try
            {
                //This is my connection string i have assigned the database file address path
                string connString = "DataSource=127.0.0.1;Database=new_schema2;UserID=root;Password=165139420;PORT=3306;logging=True;allow batch=true";
                MySqlConnection conn = new MySqlConnection(connString);
                MySqlCommand myCommand = new MySqlCommand("SELECT * FROM new_schema2.students");
                myCommand.Connection = conn;
                conn.Open();
                MySqlDataReader reader;
                reader = myCommand.ExecuteReader();

                var list = new List<Students>();
                while (reader.Read())
                {
                    if (Convert.ToInt32(reader["id"]) != null)
                    {
                        int courseId = Convert.ToInt32(reader["course_id"]);
                        var userid = Convert.ToInt32(reader["user_id"]);
                        if (userid > 14035)
                        {
                            userid = userid + 3000;
                        }

                        if (courseId > 79)
                        {
                            courseId = courseId + 971;
                        }

                        var model = new Students()
                        {
                            //ID = Convert.ToInt32(reader["id"]),
                            UserId = userid,
                            CourseId = courseId,

                        };


                        list.Add(model);
                    }

                }
                myCommand.Connection.Close();
                foreach (var item in list)
                {
                    if (item.CourseId == 5)
                    {
                        item.CourseId = 6;
                    }
                    if (item.CourseId != 10 && item.CourseId != 11 && item.CourseId != 12)
                    {
                        _context.Students.Insert(item);
                        _context.Save();
                    }

                }


            }
            catch (Exception ex)
            {

            }
        }

        public ActionResult Footer()
        {
            return PartialView("/Views/" + BaseWebsite.WebsiteLabel + "/shared/_footer.cshtml");
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}