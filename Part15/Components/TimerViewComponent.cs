using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Part15.Components
{
    // Есть три способа определения компонента:
    // 1. Определение компонента как обычного класса(класс POCO)
    // 2. Наследование от базового класса ViewComponent
    // 3. Применение к классу атрибута[ViewComponent]
    public class Time : ViewComponent
    {
        // ContentViewComponentResult: применяется для отправки текстового контента. Для создания объекта используется метод Content()
        public IViewComponentResult Invoke()
        {
            return Content(DateTime.Now.ToString("hh:mm:ss"));
        }
    }
    public class TimeBold : ViewComponent
    {
        // HtmlContentViewComponentResult: представляет фрагмент кода HTML, который инкорпорируется в веб-станицу
        // для генерации html-фрагментов, которые вставляются в основное представление, применяется класс HtmlContentViewComponentResult
        public IViewComponentResult Invoke()
        {
            return new HtmlContentViewComponentResult(new HtmlString($"<b>Текущее вермя {DateTime.Now.ToString("hh:mm:ss")}</b>"));
        }
    }
    public class GetSecondsViewComponent
    {
        public string Invoke(int delay)
        {
            return (DateTime.Now.Second + delay).ToString();
        }
    }

    [ViewComponent]
    public class Today
    {
        public string Invoke()
        {
            return DateTime.Now.ToString("dd.hh.yy");
        }
    }

    // ViewViewComponentResult: используется для генерации представления Razor с возможностью передачи модели.
    // Для создания этого объекта может применяться метод View() класса ViewComponent
    public class UsersList : ViewComponent
    {
        List<string> users;
        public UsersList()
        {
            users = new List<string>()
            {
                "Tom", "Bob", "Ivan"
            };
        }
        public IViewComponentResult Invoke()
        {
            return View(users);
        }
    }

    public class NewUsersList : ViewComponent
    {
        List<string> users;
        public NewUsersList()
        {
            users = new List<string>()
            {
                "Tom", "Bob", "Ivan"
            };
        }
        public IViewComponentResult Invoke()
        {
            int number = users.Count();
            if (Request.Query.ContainsKey("number"))
            {
                Int32.TryParse(Request.Query["number"].ToString(), out number);
            }
            ViewBag.Users = users.Take(number);
            ViewData["Header"] = $"Количество = {users.Count}";
            return View();
        }
    }

    // Иногда возникает необходимость в компоненте выполнить некоторую асинхронную операцию,
    // например, для обращения к базе данных, к внешнему сетевому ресурсу, чтению файла и т.д.
    public class Header : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string htmlContent = String.Empty;
            using (StreamReader reader = new StreamReader("Files/header.html"))
            {
                htmlContent = await reader.ReadToEndAsync();
            }
            return new HtmlContentViewComponentResult(new HtmlString(htmlContent));
        }
    }
}
