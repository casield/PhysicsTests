using System;
using System.Runtime.CompilerServices;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.CollisionDetection;
using BepuPhysics.Constraints;
using BepuUtilities.Memory;

namespace TEs_Physics
{
    class Program
    {
        public BufferPool BufferPool { get; private set; }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            new Game();
        }
    }
}
