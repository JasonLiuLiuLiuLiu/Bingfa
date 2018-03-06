using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bingfa.Model;
using Microsoft.AspNetCore.Mvc;

namespace Bingfa.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private SchoolContext schoolContext;

        public ValuesController(SchoolContext _schoolContext)
        {
            schoolContext = _schoolContext;
        }
        
        // GET api/values/5
        [HttpGet("{id}")]
        public Student Get(int id)
        {
            return schoolContext.students.Where(p => p.id == id).FirstOrDefault();
        }
        [HttpGet]
        public List<Student> Get()
        {
            return schoolContext.students.ToList();
        }

        // POST api/values
        [HttpPost]
        public string Post(Student student)
        {
            if (student.id != 0)
            {
                try
                {
                    Student studentDataBase = schoolContext.students.Where(p => p.id == student.id).FirstOrDefault();
                    if (studentDataBase.LastChanged.ToString("yyyy-MM-dd HH:mm:ss.fff").Equals(student.LastChanged.ToString("yyyy-MM-dd HH:mm:ss.fff")))
                    {
                        studentDataBase.LastChanged=DateTime.Now;
                        studentDataBase.Age = student.Age;
                        studentDataBase.Name = student.Name;
                        studentDataBase.Pwd = student.Pwd;
                        schoolContext.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("数据已经修改，请刷新查看");
                        //return "";
                    }
                }
                catch (Exception e)
                {
                    return e.Message;
                }
                return "success";
            }
            return "没有找到该Student";
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
