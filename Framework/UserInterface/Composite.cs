using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Geometry;
using Drawing;

namespace UserInterface
{
    public abstract class Composite : Visual
    {
        private class VisualCollection : Collection<Visual>
        {
            private Composite owner;

            public VisualCollection(Composite owner)
            {
                this.owner = owner;
            }

            protected override void ClearItems()
            {
                foreach (var item in this)
                {
                    this.OnRemoveVisual(item);
                }

                base.ClearItems();
            }

            protected override void InsertItem(int index, Visual visual)
            {
                this.OnAddVisual(visual);
                base.InsertItem(index, visual);
            }

            protected override void RemoveItem(int index)
            {
                this.OnRemoveVisual(this[index]);
                base.RemoveItem(index);
            }

            protected override void SetItem(int index, Visual visual)
            {
                this.OnRemoveVisual(this[index]);
                this.OnAddVisual(visual);
                base.SetItem(index, visual);
            }

            private void OnAddVisual(Visual visual)
            {
                if (visual == null)
                {
                    throw new Exception("Can not add null as a child.");
                }

                if (visual.Parent != null)
                {
                    throw new Exception("Visual allready a child of another Visual");
                }

                visual.Parent = this.owner;
            }

            private void OnRemoveVisual(Visual visual)
            {
                visual.Parent = null;
            }
        }

        public Composite()
        {
            this.Children = new VisualCollection(this);
        }

        protected IList<Visual> Children { get; private set; }

        public override IEnumerator<Visual> GetEnumerator()
        {
            return this.Children.GetEnumerator();
        }
    }
}
