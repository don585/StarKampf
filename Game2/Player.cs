﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Diagnostics;

namespace Game2
{
    class Player

    {
        enum Commands
        {
            iniUnit = 0,
            moveUnit = 1
        }

        enum Units
        {
            unicorn = 0,
        }

        
        private const double timeDivider = 30;
<<<<<<< HEAD
     
=======
        private Rectangle wallBound;

        private KeyboardState keyboardState; // for testing

>>>>>>> b18ae7c72ae02a983cb476a20c25a782941a7e3c
        //
        private GraphicsDevice GraphicsDevice;

        //using for drawing object on the map;
        private Texture2D[] allTextures;
        Texture2D wallTexture;
        private SpriteBatch sprite;
        private Vector2 spriteOrigin;// Центр спрайта
        private int side; // определяет сторону игрока .

        //
        

        //
        public Interface Inter;
        public Map map;
        //
        private double interval;
        private int[][] IntCommands;
        bool DG = false;
        private Stopwatch sw;
        private string UnitSItuation = string.Empty; // Тут собираются все взаимодействия между юнитами, картой , потом это отправляется серверу

        public Player(GraphicsDevice GraphicsDevice)
        {
            this.GraphicsDevice = GraphicsDevice;
        }

        public void Initialize()
        {
          
            sw = new Stopwatch();
            //
            //
            VecUnits = new List<BaseUnit>();
            VecUnits.Capacity = 128;
            //ini timer 
            map = new Map();
            Inter = new Interface(GraphicsDevice, 0, map); // Обязательно исправить когда будет корректная инициализация сервером.
        }


        public void Update()
        {
            interval = sw.ElapsedMilliseconds / timeDivider;
            sw.Reset(); // reset the timer (change current time to 0)
            sw.Start();
<<<<<<< HEAD
           
=======
            while ((inMsg = client.ReadMessage()) != null)
            {
                switch (inMsg.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        ReadMsg();
                        AnalyzeCommands();
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        // handle connection status messages


                        break;
                    default:
                        Console.WriteLine("unhandled message with type: "
                            + inMsg.MessageType);
                        break;

                }
                if (client.ConnectionStatus == NetConnectionStatus.Connected && DG == false)
                {
                    SendMsgIniUnit(0, 384, 384);
                    DG = true;
                }
                //
                client.Recycle(inMsg);
            }
>>>>>>> b18ae7c72ae02a983cb476a20c25a782941a7e3c

            //Тут все очень просто. Если интерфейс возвращает непустую строку, значит там команды взаимодействия.
            //Отправляем их
           

            foreach (BaseUnit Unit in VecUnits)
            {
                Unit.Act(interval, map);
            }
        }

        public void Draw()
        {
<<<<<<< HEAD
            map.DrawMap(GraphicsDevice, Inter.camera);

            Inter.DrawHealthUnderAllUnit(VecUnits, allTextures);
            
=======
            // Who will engage with Interface class?
            // You shoud draw it there;
            DrawMap();
>>>>>>> b18ae7c72ae02a983cb476a20c25a782941a7e3c
            DrawUnits();
            Inter.Draw();
        }



        private int DrawUnits()
        {
            sprite.Begin(SpriteSortMode.BackToFront,
                       BlendState.AlphaBlend,
                       null,
                       null,
                       null,
                       null,
                       Inter.camera.GetTransformation(GraphicsDevice));
<<<<<<< HEAD
=======

>>>>>>> b18ae7c72ae02a983cb476a20c25a782941a7e3c
            for (int i = 0; i < VecUnits.Count; i++)
            {
                int id = VecUnits[i].id;
                spriteOrigin = new Vector2(allTextures[id].Width / 2, allTextures[id].Height / 2);

                sprite.Draw(allTextures[id], new Vector2((int)VecUnits[i].X, (int)VecUnits[i].Y),
                    null, Color.White, VecUnits[i].Angle, spriteOrigin, 1f, SpriteEffects.None, 0f);
            }
            sprite.End();

            
            return 0;
        }

        private int DrawMap()
        {
            sprite.Begin(SpriteSortMode.BackToFront,
                                   BlendState.AlphaBlend,
                                   null,
                                   null,
                                   null,
                                   null,
                                   Inter.camera.GetTransformation(GraphicsDevice));

            for (int i = 0; i < map.width; i++)
            {
                for (int j = 0; j < map.height; j++)
                {
                    if (map[i, j] == 1)
                    {
                        Rectangle tmp = new Rectangle(i * map.tileWidth, j * map.tileHeight, map.tileWidth, map.tileHeight);
                        sprite.Draw(wallTexture, tmp, Color.White);
                    }
                }
            }
            sprite.End();
            return 0;
        }

        public void IniTextures(Texture2D[] texture, Texture2D wallTexture)
        {
            allTextures = texture;
            this.wallTexture = wallTexture;
        }

       


       


<<<<<<< HEAD
        
=======
        private void ReadMsg()
        {
            /*Приходит строка где записаны команды вот в таком формате разделенные символом перехода на следующую строку :
            * gametime
            * IDofCMD IN param
            * -//-
            * IDpfCMD - айди команды, IN идентификационый номер юнита, param параметры к команде.
            * Итого на выходе из функции имеем зубчатый массив в котором каждая строка это одна команда.
             */
            string ListOfCmd = inMsg.ReadString();
            LogMsg("Receive message : " + ListOfCmd);
            // GetIntervalFromMsg( ref ListOfCmd);
            IntCommands = new int[ListOfCmd.Split('\n').Count()][];
            int i = 0;
            foreach (string A in ListOfCmd.Split('\n'))
            {
                IntCommands[i] = A.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(n => int.Parse(n))
                .ToArray();
                i++;
            }
        }
        private void AnalyzeCommands()
        {
            //Просто пробегаем по массиву и выполняем комманды
            for (int i = 0; i < IntCommands.Count() - 1; i++)
            {
                if (IntCommands[i].Count() == 0)
                    return;
                //Определяем тип комманды
                switch ((Commands)(IntCommands[i][0]))
                {
                    case Commands.iniUnit:
                        IniUnit(i);
                        break;

                    case Commands.moveUnit://1 0 100 100 означает переместить юнит с ИН 0 в точку х = 100 у = 100;
                        MoveUnit(i);
                        
                        break;

                    default:
                        break;


                }
            }
        }

        private void IniUnit(int NumberOfCurCMS)
        {
            switch ((Units)IntCommands[NumberOfCurCMS][1])
            {
                case Units.unicorn:
                    //Temporary there is characteristic(???)  reading from .txt file.
                    // Someone should make the same , but from .db file
                    // as soon as possible

                    int[] arr = System.IO.File.ReadAllText("Units/unicorn.txt").Split(' ').Select(n => int.Parse(n)).ToArray();

                    VecUnits.Add(new Fighter(IntCommands[NumberOfCurCMS][1],
                                             IntCommands[NumberOfCurCMS][2],
                                             IntCommands[NumberOfCurCMS][3],
                                             IntCommands[NumberOfCurCMS][4],
                                             IntCommands[NumberOfCurCMS][5],
                                             "unicorn", arr[0], arr[1], arr[2], arr[3]));

                    break;


                default:
                    break;
            }
        }

        private void MoveUnit(int NumberOfCurCMS)
        {
            BaseUnit movingUnit = FindInList(VecUnits, IntCommands[NumberOfCurCMS][1]);
            movingUnit.SetMoveDest(IntCommands[NumberOfCurCMS][2], IntCommands[NumberOfCurCMS][3], map);

        }

        private BaseUnit FindInList(List<BaseUnit> VecUnits, int IN)
        {
            for (int i = 0; i < VecUnits.Count; i++)
            {
                if (VecUnits[i].GN == IN)
                    return VecUnits[i];
            }
            return null;

        }


        public void LogMsg(string message)
        {
            File.AppendAllText("log.txt", message);
        }
>>>>>>> b18ae7c72ae02a983cb476a20c25a782941a7e3c
    }


}
