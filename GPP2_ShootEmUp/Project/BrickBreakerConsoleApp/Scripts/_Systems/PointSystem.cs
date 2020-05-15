using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BrickBreakerConsoleApp
{
    class PointSystem : ISystem
    {
        public int points = 0;
        private bool updateScore;
        int start;
        int end;
        //int pointsToAdd = 50;
        public int EaseResult = 0;
        float duration = 3f;

        LTimer timer = new LTimer();
        uint startTick = 0;

        List<PointsEventData> events = new List<PointsEventData>();
        List<String> scores = new List<string>();
        string myTime = "";
        string score = "";

        private PointSystem() { }
        private static PointSystem instance = null;
        public static PointSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PointSystem();
                }
                return instance;
            }
        }

        public void Init()
        {
            timer.Start();
            EventSystem.Instance.AddListener("ShotEnemy", new EventSystem.EventListener(false)
            {
                Method = (object[] parameters) => {
                    start = points;
                    PointsEventData eData = new PointsEventData(100);
                    events.Add(eData);
                }
            });
        }


        public void SaveScore()
        {
            int test;
            GetHighScores();
                if (GetScore() == null || GetScore() == "")
                {
                    test = 0;
                }
                else
                {
                    test = int.Parse(GetScore());
                }

                using (StreamWriter writer = new StreamWriter("score.txt"))
                {
                    if (test <=  EaseResult)
                    {
                        myTime = System.DateTime.Now.ToShortDateString();
                        writer.WriteLine( EaseResult);
                        writer.WriteLine(myTime);
                    }
                    else
                    {
                        writer.WriteLine(test);
                        writer.WriteLine(myTime);
                    }
                }
            
            
        }

        public String GetTime()
        {
            return myTime;
        }
        public String GetScore()
        {
            return score;
        }
        public void GetHighScores()
        {
            String line;
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader("score.txt");
                if (sr == null  )
                {
                    StreamWriter writer = new StreamWriter("score.txt");
                    sr = new StreamReader("score.txt");
                }

                //Read the first line of text
                line = sr.ReadLine();
                score = line;
                //Continue to read until you reach end of file
                
                while (line != null)
                {
                    //write the lie to console window
                    //Console.WriteLine(line);
                    //Read the next line
                    line = sr.ReadLine();
                    myTime = line;
                }
                
                
                //close the file
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }


        public void Update()
        {
            float t = (timer.GetTicks() - startTick);

            foreach (var e in events)
            {
                int pointsToAdd = 50 * events.Count;
                //Console.WriteLine("eventcount: " + events.Count);
                if (updateScore)
                {
                    end += pointsToAdd;
                }
                else
                {
                    end = start + pointsToAdd;
                    startTick = timer.GetTicks();
                    EaseResult = 0;
                }
                updateScore = true;
            }

            if (updateScore)
            {
                float percentage = MathHelper.Clamp(t / (duration * 1000), 0, 1);
                //Console.WriteLine("Percentage: " + percentage);
                EaseResult = (int)(start + Ease.SineInOut(percentage) * (end - start));
               // Console.WriteLine("EaseResult: " + EaseResult);

                if (percentage == 1)
                {
                    start = end;
                    updateScore = false;
                    startTick = timer.GetTicks();
                    points = EaseResult;
                }
            }
            
            GameObjectSystem.Instance.GetGameObject("points").GetComponent<TextComponent>().Text.SetText(""+EaseResult);
            events.Clear();
            
        }

    }
}
