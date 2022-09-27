using System;

namespace abelkhan.admin
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Controller : Attribute
    {
        public Controller(string route)
        {
            this.route = route;
        }

        /// <summary>
        /// 控制器路由规则字符串
        /// </summary>
        public string route { get; set; } 
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class Post : Attribute {
        public Post(string route)
        {
            this.route = route;
        }

        /// <summary>
        /// Post路由规则字符串
        /// </summary>
        public string route { get; set; }

        // public static string name { get { return "Post"; } }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class Get : Attribute
    {
        public Get(string route)
        {
            this.route = route;
        }

        /// <summary>
        /// Get路由规则字符串
        /// </summary>
        public string route { get; set; }

        // public static string name { get { return "Get"; } }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class Delete : Attribute
    {
        public Delete(string route)
        {
            this.route = route;
        }

        /// <summary>
        /// Get路由规则字符串
        /// </summary>
        public string route { get; set; }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class Update : Attribute
    {
        public Update(string route)
        {
            this.route = route;
        }

        /// <summary>
        /// Update路由规则字符串
        /// </summary>
        public string route { get; set; }
    }
}
