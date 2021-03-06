using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.CollisionDetection;
using BepuPhysics.Constraints;
using BepuUtilities;
using BepuUtilities.Memory;


namespace TEs_Physics
{
    class Game
    {
        private BodyDescription boxDescription;
        private BodyHandle bodyHandle;

        public Simulation Simulation { get; }
        public IThreadDispatcher ThreadDispatcher { get; private set; }

        public Game()
        {
            Console.WriteLine("Inside Game");
            BufferPool bufferPool = new BufferPool();

            var targetThreadCount = Math.Max(1, Environment.ProcessorCount > 4 ? Environment.ProcessorCount - 2 : Environment.ProcessorCount - 1);
            ThreadDispatcher = new SimpleThreadDispatcher(targetThreadCount);

            Simulation = Simulation.Create(bufferPool, new DemoNarrowPhaseCallbacks(), new DemoPoseIntegratorCallbacks(new Vector3(0, -10, 0)), new PositionFirstTimestepper());
            createFloor();
            createObjects();

            GameLoop gameloop = new GameLoop();
            gameloop.Load(this);
            gameloop.Start().Wait();
        }

        private void createObjects()
        {
            for (int a = 0; a < 7000; a++)
            {
                var ringBoxShape = new Box(1, 1, 1);
                ringBoxShape.ComputeInertia(1, out var ringBoxInertia);
                var boxDescription = BodyDescription.CreateDynamic(new Vector3(), ringBoxInertia,
                    new CollidableDescription(Simulation.Shapes.Add(ringBoxShape), 0.1f),
                    new BodyActivityDescription(0.01f));

                boxDescription.Pose = new RigidPose(new Vector3(1, 9, 10+a), new Quaternion(0, 0, 0, 1));
                bodyHandle = Simulation.Bodies.Add(boxDescription);
            }

        }

        private void createFloor()
        {
            Simulation.Statics.Add(new StaticDescription(new Vector3(0, -0.5f, 0), new CollidableDescription(Simulation.Shapes.Add(new Box(500, 1, 500)), 0.1f)));
        }

        internal void Load()
        {
            Console.WriteLine("Load content");
        }

        internal void Update(TimeSpan time)
        {
            Simulation.Timestep(1 / 60f, ThreadDispatcher);
            Simulation.Bodies.GetDescription(bodyHandle, out boxDescription);
            Console.WriteLine(boxDescription.Pose.Position);
           

        }
    }
}
