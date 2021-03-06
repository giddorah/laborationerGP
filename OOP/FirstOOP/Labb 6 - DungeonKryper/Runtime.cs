﻿using Labb_6___DungeonKryper.Classes.Environment;
using Labb_6___DungeonKryper.Classes.Lists;
using Labb_6___DungeonKryper.Classes.Objects;
using Labb_6___DungeonKryper.Interfaces;
using Labb_6___DungeonKryper.Other_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_6___DungeonKryper
{
    class Runtime
    {
        public MyLists myLists = new MyLists();
        //public MyLists MyListsProperties { get; set; }
        public string Exits { get; set; }

        public void Start(Runtime runtime, PlayerControls playerControls)
        {
            var currentLocation = new Location();
            var userInterface = new UI();
            var map = new Map();
            var wornEquipment = new Worn();
            var equipmentHandler = new EquipmentHandler();

            NewGameReset.ResetGame(currentLocation);

            userInterface.ShownUserInterface(
                map, 
                runtime, 
                userInterface, 
                currentLocation, 
                playerControls,
                myLists.Environment,
                wornEquipment,
                equipmentHandler
                
                );
        }

        internal void ExitUpdater(Location currentLocation, Runtime runtime, int currentRoomNumber)
        {
            foreach (var location in myLists.Environment)
            {
                if (currentRoomNumber == location.RoomNumber)
                {
                    LocationUpdater(currentLocation, location);
                }
            }

            if (currentLocation.PossibleExitNorth > 0)
            {
                Exits = Exits + "North ";
            }
            if (currentLocation.PossibleExitSouth > 0)
            {
                Exits = Exits + "South ";
            }
            if (currentLocation.PossibleExitWest > 0)
            {
                Exits = Exits + "West ";
            }
            if (currentLocation.PossibleExitEast > 0)
            {
                Exits = Exits + "East ";
            }
        }

        internal void UpdateList(int currentRoomNumber)
        {
            if (currentRoomNumber == 0)
                return;
            else
                myLists.NonPlayerCharacters = myLists.
                                                Environment[currentRoomNumber - 1000].
                                                RoomContent;
        }

        public string RoomDescription(Map map, Location currentLocation, int currentRoomX, int currentRoomY, int currentRoomNumber, Runtime runtime, UI userInterface)
        {
            foreach (var location in myLists.Environment)
            {
                if (currentRoomNumber == location.RoomNumber)
                {
                    LocationUpdater(currentLocation, location);
                    Console.WriteLine("You see {0}", location.Description);
                    return String.Format("The room contains: {0}", RoomContents(runtime, 
                                                                                currentRoomNumber, 
                                                                                userInterface));
                }
            }
            return "You can't see an exit that way. Try again.";
        }



        public void LocationUpdater(Location currentLocation, IEnvironment location)
        {
            currentLocation.CurrentRoomNumber = location.RoomNumber;
            currentLocation.AvailableExits = location.Exits;
            currentLocation.PossibleExitNorth = location.ExitNorth;
            currentLocation.PossibleExitSouth = location.ExitSouth;
            currentLocation.PossibleExitWest = location.ExitWest;
            currentLocation.PossibleExitEast = location.ExitEast;
            currentLocation.CurrentRoomContents = location.Contains;
        }

        public string RoomContents(Runtime runtime, int currentRoomNumber, UI userInterface)
        {
            string numberOfAnimals = "nothing";
            foreach (var animal in myLists.Environment[currentRoomNumber - 1000].RoomContent)
            {
                if (numberOfAnimals == "nothing")
                {
                    numberOfAnimals = animal.Description;
                }
                else
                    numberOfAnimals += ", " + animal.Description;
            }
            return numberOfAnimals;
        }

        internal void PersonExaminer(Location currentLocation, Runtime runtime, UI userInterface)
        {
            foreach (var npc in myLists.NonPlayerCharacters)
            {
                if (currentLocation.CurrentRoomX == npc.ObjectLocationX && currentLocation.CurrentRoomY == npc.ObjectLocationY )
                {
                    Console.WriteLine(npc.LongDescription);
                }
                else 
                {
                    Console.WriteLine("There really is nobody to examine here.");
                    break;
                }
            }
            Console.ReadLine();
        }

        public void RoomExaminer(Location currentLocation, Runtime runtime, UI userInterface)
        {
            foreach (var room in myLists.Environment)
            {
                if (currentLocation.CurrentRoomNumber == room.RoomNumber)
                {
                    Console.WriteLine(room.LongDescription);
                }
                else if (currentLocation.CurrentRoomNumber == 0)
                {
                    Console.WriteLine("There really is nothing to see here.");
                    break;
                }
            }
            Console.ReadLine();
        }

        public void MobListener(Location currentLocation, Runtime runtime, UI userInterface)
        {
            if (myLists.NonPlayerCharacters.Count == 0)
            {
                Console.WriteLine("You have nothing to listen to.");
            }
            foreach (var animal in myLists.NonPlayerCharacters)
            {
                Console.WriteLine("The {0} says: {1}", animal.Name, animal.Speak);
            }
            Console.ReadLine();
        }
    }
}
