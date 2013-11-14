using Drawing;
using Geometry;
using System.Collections.Generic;

namespace UserInterface.Controls
{
    public class Control : Composite
    {
        private Visual root;
        private Tempalte template;
        private bool invalidTemplate;

        public Control()
        {
        }

        public Tempalte Template
        {
            get
            {
                return this.template;
            }

            set
            {
                if (this.template != value)
                {
                    this.template = value;
                    this.invalidTemplate = true;
                    // TODO: InvalidateMeasure();
                }
            }
        }

        protected Visual Root
        {
            get
            {
                return this.root;
            }

            private set
            {
                if (this.root != value)
                {
                    if (this.root != null)
                    {
                        this.Children.Remove(this.root);
                    }

                    this.root = value;
                    if (this.root != null)
                    {
                        this.Children.Add(this.root);
                    }
                }
            }
        }

        protected override Size MeasureContent(Size size, IDrawingContext context)
        {
            if (this.invalidTemplate)
            {
                if (this.Template != null)
                {
                    this.Root = this.Template.Create();
                }

                this.invalidTemplate = false;
            }

            return base.MeasureContent(size, context);
        }
    }
}
