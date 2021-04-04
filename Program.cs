using System;
using NLog.Web;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BlogsConsole
{
    class Program
    {
        // create static instance of Logger
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            string response = "";
            string name = "";
            int rating;
            int id;
            string content = "";
            string title= "";
            bool isValid = true;
            logger.Info("Program started");

            try
            {
                do
                {  
                    System.Console.WriteLine("\nEnter your selection: \n1) Display all Blogs \n2) Add a Blog \n3) Display Posts \n4) Create a Post \nEnter any other to key to quit");
                    response = Console.ReadLine();

                    if(response == "1"){
                        // Displaying Blog
                        using (var db = new BloggingContext())
                        {
                            foreach(var blog in db.Blogs)
                            {
                                System.Console.WriteLine("\nName:" + blog.Url + " \nRating:" + blog.Rating);
                            }
                        }
                    } else if (response == "2"){
                        System.Console.WriteLine("What is the name of the Blog?");
                        name = Console.ReadLine();

                        System.Console.WriteLine("What rating would you give the Blog? (1-5)");
                        try
                        {
                            rating = Int32.Parse(Console.ReadLine());
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex.Message);
                            throw new Exception("Please input a valid response!");
                        }

                        using (var db = new BloggingContext())
                        {
                            var blog = new Blog()
                            {
                                Url = name,
                                Rating = rating
                            };
                            db.Blogs.Add(blog);
                            db.SaveChanges();
                        }

                    } else if (response == "3"){
                    System.Console.WriteLine("Which blog would you like to see the Posts of? (BlogId)");

                        using (var db = new BloggingContext())
                            {
                                foreach(var blog in db.Blogs)
                                {
                                    System.Console.WriteLine("\nBlog ID: " + blog.BlogId + " Name: " + blog.Url);
                                }

                                try
                                {
                                    id = Int32.Parse(Console.ReadLine());
                                }
                                catch (Exception ex)
                                {
                                    logger.Error(ex.Message);
                                    throw new Exception("Please input a valid response!");
                                }
                                

                                var blogs = db.Blogs
                                    .Include(b => b.Posts).ToList();
                            
                                foreach(var blog in blogs)
                                {
                                blog.Posts.Where(x => x.BlogId.Equals(id))
                                .ToList()
                                .ForEach( x => 
                                {
                                    if(id == x.BlogId)
                                    {
                                        System.Console.WriteLine($"({x.PostId}) {x.Title}");
                                    }
                                });
                            }

                                }                            
                        
                    } else if (response == "4"){
                        System.Console.WriteLine("Which Blog would you like to create a Post for?(BlogId)");
                        using (var db = new BloggingContext())
                            {
                                foreach(var blog in db.Blogs)
                                {
                                    System.Console.WriteLine("\nBlog ID: " + blog.BlogId + " Name: " + blog.Url);
                                }
                                
                                try
                                {
                                    id = Int32.Parse(Console.ReadLine());
                                }
                                catch (Exception ex)
                                {
                                    logger.Error(ex.Message);
                                    throw new Exception("Please input a valid response!");
                                }

                                System.Console.WriteLine("Enter a Title for your post");
                                title =  Console.ReadLine();

                                System.Console.WriteLine("Please Enter your Posts Contents");
                                content = Console.ReadLine();

                                var post = new Post(){
                                    BlogId = id,
                                    Content = content,
                                    Title = title
                                };
                                db.Posts.Add(post);
                                db.SaveChanges();
                            }
                    }else{
                        System.Console.WriteLine("Thank you for using our services");
                        isValid = false;
                    }
                
                } while (isValid);
                
                // // Create and save a new Blog
                // Console.Write("Enter a name for a new Blog: ");
                // var name = Console.ReadLine();

                // var blog = new Blog { Name = name };

                // var db = new BloggingContext();
                // db.AddBlog(blog);
                // logger.Info("Blog added - {name}", name);

                // // Display all Blogs from the database
                // var query = db.Blogs.OrderBy(b => b.Name);

                // Console.WriteLine("All blogs in the database:");
                // foreach (var item in query)
                // {
                //     Console.WriteLine(item.Name);
                // }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

            logger.Info("Program ended");
        }
    }
}
