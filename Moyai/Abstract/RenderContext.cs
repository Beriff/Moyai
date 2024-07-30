﻿using Moyai.Impl;
using Moyai.Impl.Input;
using Moyai.Impl.Math;

namespace Moyai.Abstract
{
    public abstract class RenderContext
    {
        private DateTime PrevTimestamp { get; set; }
        public double TimeDelta { get; private set; }
        public bool Finished { get; set; } = false;
        public virtual void Render()
        {
			//Calculate time delta
			TimeDelta = (DateTime.Now - PrevTimestamp).TotalSeconds;
            PrevTimestamp = DateTime.Now;

            InputHandler.Update();
        }

        protected RenderContext()
        {
            Console.CursorVisible = false;
            PrevTimestamp = DateTime.Now;
        }

        public void Start()
        {
            while(!Finished)
            {
                Render();
            }
        }

    }
}
