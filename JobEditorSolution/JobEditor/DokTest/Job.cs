using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace JobEditor.DokTest
{
    public class Job :IComparable<Job>
    {
        public enum Feature
        {
            CUT_90,
            CUT_45,
            CUT_45_TC_LEFT,
            CUT_135,
            V_TOP,
            V_BOTTOM,
            C_LEFT,
            C_RIGHT,
            H_OBLONG,
            H_ROUND
        }
        public class ShapePart
        {
            public Feature Feature { get; set; }
            public double OverCut { get; set; }
            public bool DoubleCut { get; set; }
            public double X { get; set; }
            public double Y { get; set; }
        }
        public class Shape
        {
            public int No { get; set; }
            public IList<ShapePart> Parts { get; set; }
        }
        public class Layer
        {
            public int Id { get; set; }
            public int ShapesPerProduct { get; set; }
            public IList<Shape> Shapes { get; set; }
        }
        public String Name { get; set; }
        public IList<Layer> Layers { get; set; }

        public int CompareTo(Job other)
        {
            if (!this.Name.Equals(other.Name))
                return this.Name.CompareTo(other.Name);
            if (this.Layers.Count != other.Layers.Count)
                return this.Layers.Count.CompareTo(other.Layers.Count);
            for (int i = 0; i < this.Layers.Count; i++)
            {
                if(this.Layers[i].Id != other.Layers[i].Id)
                    return this.Layers[i].Id.CompareTo(other.Layers[i].Id);
                if (this.Layers[i].ShapesPerProduct != other.Layers[i].ShapesPerProduct)
                    return this.Layers[i].ShapesPerProduct.CompareTo(other.Layers[i].ShapesPerProduct);
                if (this.Layers[i].Shapes.Count != other.Layers[i].Shapes.Count)
                    return this.Layers[i].Shapes.Count.CompareTo(other.Layers[i].Shapes.Count);
                for (int j = 0; j < this.Layers[i].Shapes.Count; j++)
                {
                    if (this.Layers[i].Shapes[j].No != other.Layers[i].Shapes[j].No)
                        return this.Layers[i].Shapes[j].No.CompareTo(other.Layers[i].Shapes[j].No);
                    if(this.Layers[i].Shapes[j].Parts.Count != other.Layers[i].Shapes[j].Parts.Count)
                        return this.Layers[i].Shapes[j].Parts.Count.CompareTo(other.Layers[i].Shapes[j].Parts.Count);
                    for (int k = 0; k < this.Layers[i].Shapes[j].Parts.Count; k++)
                    {
                        if (this.Layers[i].Shapes[j].Parts[k].X != other.Layers[i].Shapes[j].Parts[k].X)
                            return this.Layers[i].Shapes[j].Parts[k].X.CompareTo(other.Layers[i].Shapes[j].Parts[k].X);
                        if(this.Layers[i].Shapes[j].Parts[k].Feature != other.Layers[i].Shapes[j].Parts[k].Feature)
                            return this.Layers[i].Shapes[j].Parts[k].Feature.CompareTo(other.Layers[i].Shapes[j].Parts[k].Feature);
                        if(this.Layers[i].Shapes[j].Parts[k].Y != other.Layers[i].Shapes[j].Parts[k].Y)
                            return this.Layers[i].Shapes[j].Parts[k].Y.CompareTo(other.Layers[i].Shapes[j].Parts[k].Y);
                    }
                }
            }
            return 0;
        }
    }
}
