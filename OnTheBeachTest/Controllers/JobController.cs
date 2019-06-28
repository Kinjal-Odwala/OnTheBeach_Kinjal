using OnTheBeachTest.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnTheBeachTest.Controllers
{
    public class JobController : Controller
    {
        //Dictionary for Storing input Values
        static ConcurrentDictionary<string, string> dictionary = new ConcurrentDictionary<string, string>();
       
        public ActionResult Jobs()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Jobs(Jobs job)
        {
            string input = job.InputValue;
            ViewBag.Output = "";
            string output = "";
            string temp = "";

            if (input != null)
            {
                // Split rows
                string[] lines = input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                foreach (string line in lines)
                {
                    if (line.Length > 0)
                    {
                        if (!line.Contains("=>"))
                        {
                            // Show error : Show dependancy ; Missing =>
                            ModelState.AddModelError("InputValue", "Please enter valid jobs. Please enter valid Input");
                            return View();
                        }
                        else
                        {
                            string[] splits = line.Split(new string[] { "=>" }, StringSplitOptions.None);
                            //Insert job and its dependancy in the dictionary for further manipulation
                            dictionary.TryAdd(splits[0].Trim(), splits[1].Trim());
                        }
                    }
                }
            }
            else
            {
                // Show error : Empty input
                ModelState.AddModelError("InputValue", "Please enter some jobs. Please enter valid Input");
                return View();
            }

            if (dictionary != null)
            {
                // Check for valid input
                if (!isInputValid())
                {
                    var reversedic = dictionary.Reverse();
                    foreach (KeyValuePair<String, String> entry in reversedic)
                    {
                        string jobChar = entry.Key;
                        string depndentChar = entry.Value;

                        // Check if Job has dependancy
                        if (depndentChar == "") // No dependency
                        {
                            if (!output.Contains(jobChar) && !temp.Contains(jobChar))
                            {
                                output += jobChar;
                            }
                        }
                        else
                        {
                            // if job exist in output string then add dependency before the job char
                            if (output.Contains(jobChar)) output = output.Insert(output.IndexOf(jobChar), depndentChar);
                            // if dependency exist then add char after the dependancy char
                            else if (output.Contains(depndentChar)) output = output.Insert(output.IndexOf(depndentChar) + 1, jobChar);
                            // if none of them exists then add dependancy before char
                            else output += depndentChar + jobChar;
                        }
                    }
                    ViewBag.Output = output + temp;
                }
            }
            dictionary.Clear();
            return View(job);
        }


        public bool isInputValid()
        {
            var rdic = dictionary.Reverse();
            foreach (KeyValuePair<String, String> entry in rdic)
            {
                string jobChar = entry.Key;
                string depndentChar = entry.Value;
                string dependancy = "";
                if (jobChar == depndentChar)
                {
                    ModelState.AddModelError("InputValue", "Jobs can’t depend on themselves. Please enter valid Input");
                    return true;
                }
                else if (depndentChar != "")
                {
                    dependancy = dictionary.FirstOrDefault(x => x.Key == depndentChar).Value;
                    do
                    {
                        if (dependancy == jobChar)
                        {
                            ModelState.AddModelError("InputValue", "Jobs can’t have circular dependencies. Please enter valid Input");
                            return true;
                        }
                        else
                        {
                            dependancy = dictionary.FirstOrDefault(x => x.Key == dependancy).Value;
                        }
                        if (dependancy == "" || dependancy == null) return false;

                    } while (dependancy != "" || dependancy != null);
                }

            }
            return false;
        }
    }
}