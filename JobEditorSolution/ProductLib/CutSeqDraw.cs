
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ProductLib
{
  public class CutSeqDraw
  {
    private double stepLapYMovement;
    private double distanceBetweenShapes;
    private string cutSeqName;

    public List<UIElement> UIElementCollection { get; set; }

    public CutSeqDraw()
    {
      this.initialize();
      this.UIElementCollection = new List<UIElement>();
    }

    public string CutSeqName
    {
      get
      {
        return this.cutSeqName;
      }
      set
      {
        this.cutSeqName = value;
      }
    }

    private void initialize()
    {
    }

    public double DistanceBetweenShapes
    {
      get
      {
        return this.distanceBetweenShapes;
      }
      set
      {
        this.distanceBetweenShapes = value;
      }
    }

    public double StepLapYMovement
    {
      get
      {
        return this.stepLapYMovement;
      }
      set
      {
        this.stepLapYMovement = value;
      }
    }

    public IList<EToolCutSequence> ConvertToEToolCutSequenceList(string[] drawingParts)
    {
      IList<EToolCutSequence> etoolCutSequenceList = (IList<EToolCutSequence>) new List<EToolCutSequence>();
      for (int index = 0; index < drawingParts.Length; ++index)
      {
        if (drawingParts[index].Equals("C") || drawingParts[index].Equals("CB") || drawingParts[index].Equals("CF"))
        {
          if (drawingParts[index].Equals("C") && index == 0)
            etoolCutSequenceList.Add(EToolCutSequence.Cl);
          if (drawingParts[index].Equals("CB") && index == 0)
            etoolCutSequenceList.Add(EToolCutSequence.CBl);
          if (drawingParts[index].Equals("CF") && index == 0)
            etoolCutSequenceList.Add(EToolCutSequence.CFl);
          if (drawingParts[index].Equals("C") && index == drawingParts.Length - 1)
            etoolCutSequenceList.Add(EToolCutSequence.Cr);
          if (drawingParts[index].Equals("CB") && index == drawingParts.Length - 1)
            etoolCutSequenceList.Add(EToolCutSequence.CBr);
          if (drawingParts[index].Equals("CF") && index == drawingParts.Length - 1)
            etoolCutSequenceList.Add(EToolCutSequence.CFr);
        }
        else if (drawingParts[index].Equals("S90") || drawingParts[index].Equals("S45") || (drawingParts[index].Equals("S135") || drawingParts[index].Equals("VB")) || (drawingParts[index].Equals("VF") || drawingParts[index].Equals("S45") || (drawingParts[index].Equals("VBS") || drawingParts[index].Equals("VFS"))))
        {
          if (drawingParts[index].Equals("S90"))
            etoolCutSequenceList.Add(EToolCutSequence.S90);
          if (drawingParts[index].Equals("S45"))
            etoolCutSequenceList.Add(EToolCutSequence.S45);
          if (drawingParts[index].Equals("S135"))
            etoolCutSequenceList.Add(EToolCutSequence.S135);
          if (drawingParts[index].Equals("VB"))
            etoolCutSequenceList.Add(EToolCutSequence.VB);
          if (drawingParts[index].Equals("VF"))
            etoolCutSequenceList.Add(EToolCutSequence.VF);
          if (drawingParts[index].Equals("VBS"))
            etoolCutSequenceList.Add(EToolCutSequence.VBS);
          if (drawingParts[index].Equals("VFS"))
            etoolCutSequenceList.Add(EToolCutSequence.VFS);
        }
        else
        {
          etoolCutSequenceList.Clear();
          return etoolCutSequenceList;
        }
      }
      return etoolCutSequenceList;
    }

    private IList<IList<EToolCutSequence>> convertCutSequence(IList<Shape> shapes)
    {
      IList<IList<EToolCutSequence>> etoolCutSequenceListList = (IList<IList<EToolCutSequence>>) new List<IList<EToolCutSequence>>();
      List<string> stringList = new List<string>();
      for (int index = 0; index < shapes.Count; ++index)
        stringList.Add(shapes[index].Drawing);
      foreach (string str in stringList)
      {
        char[] chArray = new char[1]{ '|' };
        string[] drawingParts = str.Split(chArray);
        if (drawingParts.Length != 0)
          etoolCutSequenceListList.Add(this.ConvertToEToolCutSequenceList(drawingParts));
      }
      return etoolCutSequenceListList;
    }

    private void changeCutSeqShapePartEShapePart(CutSeqShapePart sp, EToolCutSequence eShapePart)
    {
      IList<PointXY> pointXyList1 = ProjectConstants.CutSequenceModel[sp.EShapePart];
      IList<PointXY> pointXyList2 = ProjectConstants.CutSequenceModel[eShapePart];
      for (int index = 0; index < pointXyList1.Count; ++index)
      {
        double num1 = pointXyList2[sp.Points[index].PointPartIndex].X - pointXyList1[sp.Points[index].PointPartIndex].X;
        double num2 = pointXyList2[sp.Points[index].PointPartIndex].Y - pointXyList1[sp.Points[index].PointPartIndex].Y;
        sp.Points[index].X += num1;
        sp.Points[index].Y += num2;
      }
      sp.EShapePart = eShapePart;
    }

    private Polygon getShapePolygon(CutSeqShape csShape, int layerPosition, ShapeProperties shapeProperty)
    {
      Polygon polygon = new Polygon();
      SolidColorBrush solidColorBrush1 = new SolidColorBrush();
      solidColorBrush1.Opacity = shapeProperty.PolygonProperties.StrokeOpacity;
      solidColorBrush1.Color = shapeProperty.PolygonProperties.StrokeColor;
      SolidColorBrush solidColorBrush2 = new SolidColorBrush();
      solidColorBrush2.Opacity = shapeProperty.PolygonProperties.FillOpacity;
      solidColorBrush2.Color = shapeProperty.PolygonProperties.FillColor;
      polygon.StrokeThickness = shapeProperty.PolygonProperties.StrokeTicknees;
      polygon.SetValue(RenderOptions.EdgeModeProperty, (object) EdgeMode.Aliased);
      Thickness margin = polygon.Margin;
      margin.Left = shapeProperty.PolygonProperties.Margin.Left;
      margin.Top = shapeProperty.PolygonProperties.Margin.Top + this.stepLapYMovement * (double) layerPosition;
      if (csShape.Parts.Count <= 3)
      {
        ref Thickness local = ref margin;
        local.Right = local.Left + (12.0 + 2.0 * shapeProperty.PolygonProperties.DistanceBetweenParts) * shapeProperty.PolygonProperties.ScallingFactor;
      }
      else
      {
        ref Thickness local = ref margin;
        local.Right = local.Left + ((double) (csShape.Parts.Count - 1) * (shapeProperty.PolygonProperties.DistanceBetweenParts + 4.0) + 4.0) * shapeProperty.PolygonProperties.ScallingFactor;
      }
      polygon.Margin = margin;
      polygon.Stroke = (Brush) solidColorBrush1;
      polygon.Fill = (Brush) solidColorBrush2;
      foreach (CutSeqShapePart part in (IEnumerable<CutSeqShapePart>) csShape.Parts)
      {
        foreach (PointXY point in (IEnumerable<PointXY>) part.Points)
          polygon.Points.Add(new Point(point.X * shapeProperty.PolygonProperties.ScallingFactor, point.Y * shapeProperty.PolygonProperties.ScallingFactor));
      }
      return polygon;
    }

    private IList<UIElement> getHoles(CutSeqShape cutSeqShape, double leftStart, ShapeProperties shapeProperty)
    {
      IList<UIElement> uiElementList = (IList<UIElement>) new List<UIElement>();
      SolidColorBrush solidColorBrush1 = new SolidColorBrush();
      solidColorBrush1.Color = shapeProperty.HoleProperties.StrokeColor;
      SolidColorBrush solidColorBrush2 = new SolidColorBrush();
      solidColorBrush2.Color = shapeProperty.HoleProperties.FillColor;
      SolidColorBrush solidColorBrush3 = new SolidColorBrush();
      solidColorBrush3.Color = shapeProperty.HoleProperties.AnnotationProperties.LineColor;
      SolidColorBrush solidColorBrush4 = new SolidColorBrush();
      solidColorBrush4.Color = shapeProperty.HoleProperties.ArrowLineProperties.Line1Color;
      SolidColorBrush solidColorBrush5 = new SolidColorBrush();
      solidColorBrush5.Color = shapeProperty.HoleProperties.ArrowLineProperties.Line2Color;
      SolidColorBrush solidColorBrush6 = new SolidColorBrush();
      solidColorBrush6.Color = shapeProperty.HoleProperties.LabelProperties.FontColor;
      Thickness margin;
      if (cutSeqShape.ShapeHole.NumberOfHoles > 0)
      {
        double left = leftStart + (4.0 + shapeProperty.PolygonProperties.DistanceBetweenParts / 2.0) * shapeProperty.PolygonProperties.ScallingFactor - ((double) shapeProperty.HoleProperties.Diameter + shapeProperty.HoleProperties.StrokeThickness) / 2.0;
        double top1 = 2.0 * shapeProperty.PolygonProperties.ScallingFactor + shapeProperty.PolygonProperties.Margin.Top - ((double) shapeProperty.HoleProperties.Diameter + shapeProperty.HoleProperties.StrokeThickness) / 2.0;
        if (cutSeqShape.ShapeHole.Properties.AnnotationProperties.IsEnabled && (cutSeqShape.ShapeHole.MeasuringType == EMeasuringType.Absolute || cutSeqShape.ShapeHole.MeasuringType == EMeasuringType.Relative))
        {
          Line line1 = new Line();
          line1.Stroke = (Brush) solidColorBrush3;
          line1.StrokeThickness = shapeProperty.HoleProperties.AnnotationProperties.StrokeThickness;
          line1.X1 = leftStart + 2.0 * shapeProperty.PolygonProperties.ScallingFactor;
          line1.X2 = leftStart + 2.0 * shapeProperty.PolygonProperties.ScallingFactor;
          Line line2 = line1;
          double num1 = 2.0 * shapeProperty.PolygonProperties.ScallingFactor;
          margin = shapeProperty.PolygonProperties.Margin;
          double top2 = margin.Top;
          double num2 = num1 + top2 + 6.0;
          line2.Y1 = num2;
          Line line3 = line1;
          double num3 = 4.0 * shapeProperty.PolygonProperties.ScallingFactor;
          margin = shapeProperty.PolygonProperties.Margin;
          double top3 = margin.Top;
          double num4 = num3 + top3 - 7.0;
          line3.Y2 = num4;
          if (shapeProperty.HoleProperties.AnnotationProperties.AliasedON)
            line1.SetValue(RenderOptions.EdgeModeProperty, (object) EdgeMode.Aliased);
          Panel.SetZIndex((UIElement) line1, 6);
          uiElementList.Add((UIElement) line1);
        }
        if (cutSeqShape.ShapeHole.Properties.AnnotationProperties.IsEnabled && (cutSeqShape.ShapeHole.MeasuringType == EMeasuringType.Absolute || cutSeqShape.ShapeHole.MeasuringType == EMeasuringType.Relative || cutSeqShape.ShapeHole.MeasuringType == EMeasuringType.Simetric && cutSeqShape.ShapeHole.NumberOfHoles > 1))
        {
          Line line1 = new Line();
          line1.Stroke = (Brush) solidColorBrush3;
          line1.StrokeThickness = shapeProperty.HoleProperties.AnnotationProperties.StrokeThickness;
          line1.X1 = leftStart + shapeProperty.PolygonProperties.ScallingFactor * (4.0 + shapeProperty.PolygonProperties.DistanceBetweenParts / 2.0);
          line1.X2 = leftStart + shapeProperty.PolygonProperties.ScallingFactor * (4.0 + shapeProperty.PolygonProperties.DistanceBetweenParts / 2.0);
          Line line2 = line1;
          double num1 = 2.0 * shapeProperty.PolygonProperties.ScallingFactor;
          margin = shapeProperty.PolygonProperties.Margin;
          double top2 = margin.Top;
          double num2 = num1 + top2 + 6.0;
          line2.Y1 = num2;
          Line line3 = line1;
          double num3 = 4.0 * shapeProperty.PolygonProperties.ScallingFactor;
          margin = shapeProperty.PolygonProperties.Margin;
          double top3 = margin.Top;
          double num4 = num3 + top3 - 7.0;
          line3.Y2 = num4;
          if (shapeProperty.HoleProperties.AnnotationProperties.AliasedON)
            line1.SetValue(RenderOptions.EdgeModeProperty, (object) EdgeMode.Aliased);
          Panel.SetZIndex((UIElement) line1, 6);
          uiElementList.Add((UIElement) line1);
        }
        if (cutSeqShape.ShapeHole.Properties.AnnotationProperties.IsEnabled && (cutSeqShape.ShapeHole.MeasuringType == EMeasuringType.Absolute || cutSeqShape.ShapeHole.MeasuringType == EMeasuringType.Relative))
        {
          Line line1 = new Line();
          line1.Stroke = (Brush) solidColorBrush4;
          line1.StrokeThickness = shapeProperty.HoleProperties.ArrowLineProperties.StrokeThickness;
          line1.X1 = leftStart + 2.0 * shapeProperty.PolygonProperties.ScallingFactor + shapeProperty.HoleProperties.AnnotationProperties.StrokeThickness / 2.0;
          line1.X2 = leftStart + shapeProperty.PolygonProperties.ScallingFactor * (4.0 + shapeProperty.PolygonProperties.DistanceBetweenParts / 2.0) - shapeProperty.HoleProperties.AnnotationProperties.StrokeThickness / 2.0;
          Line line2 = line1;
          double num1 = 4.0 * shapeProperty.PolygonProperties.ScallingFactor;
          margin = shapeProperty.PolygonProperties.Margin;
          double top2 = margin.Top;
          double num2 = num1 + top2 - 5.0 - shapeProperty.HoleProperties.ArrowLineProperties.StrokeThickness / 2.0 - 2.0;
          line2.Y1 = num2;
          Line line3 = line1;
          double num3 = 4.0 * shapeProperty.PolygonProperties.ScallingFactor;
          margin = shapeProperty.PolygonProperties.Margin;
          double top3 = margin.Top;
          double num4 = num3 + top3 - 5.0 - shapeProperty.HoleProperties.ArrowLineProperties.StrokeThickness / 2.0 - 2.0;
          line3.Y2 = num4;
          if (shapeProperty.HoleProperties.ArrowLineProperties.AliasedON)
            line1.SetValue(RenderOptions.EdgeModeProperty, (object) EdgeMode.Aliased);
          Panel.SetZIndex((UIElement) line1, 6);
          uiElementList.Add((UIElement) line1);
        }
        Ellipse ellipse = new Ellipse();
        ellipse.Width = (double) shapeProperty.HoleProperties.Diameter;
        ellipse.Height = (double) shapeProperty.HoleProperties.Diameter;
        ellipse.Fill = (Brush) solidColorBrush2;
        ellipse.Stroke = (Brush) solidColorBrush1;
        ellipse.StrokeThickness = shapeProperty.HoleProperties.StrokeThickness;
        ellipse.Margin = new Thickness(left, top1, 0.0, 0.0);
        if (shapeProperty.HoleProperties.AliasedON)
          ellipse.SetValue(RenderOptions.EdgeModeProperty, (object) EdgeMode.Aliased);
        uiElementList.Add((UIElement) ellipse);
      }
      if (cutSeqShape.ShapeHole.NumberOfHoles > 1)
      {
        double num1 = leftStart + (4.0 + shapeProperty.PolygonProperties.DistanceBetweenParts / 2.0) * shapeProperty.PolygonProperties.ScallingFactor - ((double) shapeProperty.HoleProperties.Diameter + shapeProperty.HoleProperties.StrokeThickness) / 2.0;
        double left = cutSeqShape.Parts.Count > 3 ? leftStart + (4.0 + shapeProperty.PolygonProperties.DistanceBetweenParts / 2.0) * shapeProperty.PolygonProperties.ScallingFactor - ((double) shapeProperty.HoleProperties.Diameter + shapeProperty.HoleProperties.StrokeThickness) / 2.0 + shapeProperty.PolygonProperties.ScallingFactor * (double) (cutSeqShape.Parts.Count - 2) * (4.0 + shapeProperty.PolygonProperties.DistanceBetweenParts) : leftStart + (4.0 + shapeProperty.PolygonProperties.DistanceBetweenParts / 2.0) * shapeProperty.PolygonProperties.ScallingFactor - ((double) shapeProperty.HoleProperties.Diameter + shapeProperty.HoleProperties.StrokeThickness) / 2.0 + shapeProperty.PolygonProperties.ScallingFactor * (4.0 + shapeProperty.PolygonProperties.DistanceBetweenParts);
        double num2 = 2.0 * shapeProperty.PolygonProperties.ScallingFactor;
        margin = shapeProperty.PolygonProperties.Margin;
        double top1 = margin.Top;
        double top2 = num2 + top1 - ((double) shapeProperty.HoleProperties.Diameter + shapeProperty.HoleProperties.StrokeThickness) / 2.0;
        Ellipse ellipse = new Ellipse();
        ellipse.Width = (double) shapeProperty.HoleProperties.Diameter;
        ellipse.Height = (double) shapeProperty.HoleProperties.Diameter;
        ellipse.Fill = (Brush) solidColorBrush2;
        ellipse.Stroke = (Brush) solidColorBrush1;
        ellipse.StrokeThickness = shapeProperty.HoleProperties.StrokeThickness;
        ellipse.Margin = new Thickness(left, top2, 0.0, 0.0);
        if (shapeProperty.HoleProperties.AliasedON)
          ellipse.SetValue(RenderOptions.EdgeModeProperty, (object) EdgeMode.Aliased);
        uiElementList.Add((UIElement) ellipse);
        if (cutSeqShape.ShapeHole.Properties.AnnotationProperties.IsEnabled)
        {
          Line line1 = new Line();
          line1.Stroke = (Brush) solidColorBrush3;
          line1.StrokeThickness = shapeProperty.HoleProperties.AnnotationProperties.StrokeThickness;
          if (cutSeqShape.Parts.Count <= 3)
          {
            line1.X1 = leftStart + shapeProperty.PolygonProperties.ScallingFactor * (8.0 + 3.0 * shapeProperty.PolygonProperties.DistanceBetweenParts / 2.0);
            line1.X2 = leftStart + shapeProperty.PolygonProperties.ScallingFactor * (8.0 + 3.0 * shapeProperty.PolygonProperties.DistanceBetweenParts / 2.0);
          }
          else
          {
            line1.X1 = leftStart + shapeProperty.PolygonProperties.ScallingFactor * ((double) (4 * (cutSeqShape.Parts.Count - 1)) + ((double) cutSeqShape.Parts.Count - 1.5) * shapeProperty.PolygonProperties.DistanceBetweenParts);
            line1.X2 = leftStart + shapeProperty.PolygonProperties.ScallingFactor * ((double) (4 * (cutSeqShape.Parts.Count - 1)) + ((double) cutSeqShape.Parts.Count - 1.5) * shapeProperty.PolygonProperties.DistanceBetweenParts);
          }
          Line line2 = line1;
          double num3 = 2.0 * shapeProperty.PolygonProperties.ScallingFactor;
          margin = shapeProperty.PolygonProperties.Margin;
          double top3 = margin.Top;
          double num4 = num3 + top3 + 6.0;
          line2.Y1 = num4;
          Line line3 = line1;
          double num5 = 4.0 * shapeProperty.PolygonProperties.ScallingFactor;
          margin = shapeProperty.PolygonProperties.Margin;
          double top4 = margin.Top;
          double num6 = num5 + top4 - 7.0;
          line3.Y2 = num6;
          if (shapeProperty.HoleProperties.AnnotationProperties.AliasedON)
            line1.SetValue(RenderOptions.EdgeModeProperty, (object) EdgeMode.Aliased);
          Panel.SetZIndex((UIElement) line1, 6);
          uiElementList.Add((UIElement) line1);
        }
        if (cutSeqShape.ShapeHole.Properties.ArrowLineProperties.IsEnabled)
        {
          Line line1 = new Line();
          line1.Stroke = (Brush) solidColorBrush5;
          line1.StrokeThickness = shapeProperty.HoleProperties.ArrowLineProperties.StrokeThickness;
          line1.X1 = leftStart + shapeProperty.PolygonProperties.ScallingFactor * (4.0 + shapeProperty.PolygonProperties.DistanceBetweenParts / 2.0) + shapeProperty.HoleProperties.AnnotationProperties.StrokeThickness / 2.0;
          line1.X2 = cutSeqShape.Parts.Count > 3 ? leftStart + shapeProperty.PolygonProperties.ScallingFactor * ((double) (4 * (cutSeqShape.Parts.Count - 1)) + ((double) cutSeqShape.Parts.Count - 1.5) * shapeProperty.PolygonProperties.DistanceBetweenParts) - shapeProperty.HoleProperties.AnnotationProperties.StrokeThickness / 2.0 : leftStart + shapeProperty.PolygonProperties.ScallingFactor * (8.0 + 3.0 * shapeProperty.PolygonProperties.DistanceBetweenParts / 2.0) - shapeProperty.HoleProperties.AnnotationProperties.StrokeThickness / 2.0;
          Line line2 = line1;
          double num3 = 4.0 * shapeProperty.PolygonProperties.ScallingFactor;
          margin = shapeProperty.PolygonProperties.Margin;
          double top3 = margin.Top;
          double num4 = num3 + top3 - 5.0 - shapeProperty.HoleProperties.ArrowLineProperties.StrokeThickness / 2.0 - 2.0;
          line2.Y1 = num4;
          Line line3 = line1;
          double num5 = 4.0 * shapeProperty.PolygonProperties.ScallingFactor;
          margin = shapeProperty.PolygonProperties.Margin;
          double top4 = margin.Top;
          double num6 = num5 + top4 - 5.0 - shapeProperty.HoleProperties.ArrowLineProperties.StrokeThickness / 2.0 - 2.0;
          line3.Y2 = num6;
          if (shapeProperty.HoleProperties.ArrowLineProperties.AliasedON)
            line1.SetValue(RenderOptions.EdgeModeProperty, (object) EdgeMode.Aliased);
          Panel.SetZIndex((UIElement) line1, 6);
          uiElementList.Add((UIElement) line1);
        }
        Label label = new Label();
        if (cutSeqShape.ShapeHole.NumberOfHoles > 2 && cutSeqShape.ShapeHole.Properties.LabelProperties.IsEnabled)
        {
          label.Content = (object) ("+" + (cutSeqShape.ShapeHole.NumberOfHoles - 2).ToString());
          label.Margin = new Thickness(left - 5.0, top2 - 20.0, 0.0, 0.0);
          label.FontSize = shapeProperty.HoleProperties.LabelProperties.FontSize;
          label.Foreground = (Brush) solidColorBrush6;
          label.Width = 50.0;
          label.Height = 30.0;
          uiElementList.Add((UIElement) label);
          label.Visibility = Visibility.Visible;
          Panel.SetZIndex((UIElement) label, 7);
        }
        else
          label.Visibility = Visibility.Hidden;
      }
      return uiElementList;
    }

    private IList<UIElement> getSlots(CutSeqShape cutSeqShape, double leftStart, ShapeProperties shapeProperty)
    {
      IList<UIElement> uiElementList = (IList<UIElement>) new List<UIElement>();
      SolidColorBrush solidColorBrush1 = new SolidColorBrush();
      solidColorBrush1.Color = shapeProperty.SlotProperties.StrokeColor;
      SolidColorBrush solidColorBrush2 = new SolidColorBrush();
      solidColorBrush2.Color = shapeProperty.SlotProperties.FillColor;
      SolidColorBrush solidColorBrush3 = new SolidColorBrush();
      solidColorBrush3.Color = shapeProperty.SlotProperties.AnnotationProperties.LineColor;
      SolidColorBrush solidColorBrush4 = new SolidColorBrush();
      solidColorBrush4.Color = shapeProperty.SlotProperties.ArrowLineProperties.Line1Color;
      SolidColorBrush solidColorBrush5 = new SolidColorBrush();
      solidColorBrush5.Color = shapeProperty.SlotProperties.ArrowLineProperties.Line2Color;
      new SolidColorBrush().Color = shapeProperty.SlotProperties.ArrowLineProperties.Line3Color;
      SolidColorBrush solidColorBrush6 = new SolidColorBrush();
      solidColorBrush6.Color = shapeProperty.SlotProperties.LabelProperties.FontColor;
      if (cutSeqShape.ShapeSlot.NumberOfSlotes > 0)
      {
        double num1 = cutSeqShape.Parts.Count > 3 ? shapeProperty.PolygonProperties.ScallingFactor / 2.0 * ((double) (4 * cutSeqShape.Parts.Count) + (double) (cutSeqShape.Parts.Count - 1) * shapeProperty.PolygonProperties.DistanceBetweenParts) : shapeProperty.PolygonProperties.ScallingFactor * (6.0 + shapeProperty.PolygonProperties.DistanceBetweenParts);
        double left = leftStart + num1 - ((double) shapeProperty.SlotProperties.Width + 2.0 * shapeProperty.SlotProperties.StrokeThickness) / 2.0;
        double top1 = 2.0 * shapeProperty.PolygonProperties.ScallingFactor + shapeProperty.PolygonProperties.Margin.Top - ((double) shapeProperty.SlotProperties.Height + 2.0 * shapeProperty.SlotProperties.StrokeThickness) / 2.0;
        Thickness margin;
        if (cutSeqShape.ShapeSlot.Properties.AnnotationProperties.IsEnabled && (cutSeqShape.ShapeSlot.MeasuringType == EMeasuringType.Absolute || cutSeqShape.ShapeSlot.MeasuringType == EMeasuringType.Relative))
        {
          Line line1 = new Line();
          line1.Stroke = (Brush) solidColorBrush3;
          line1.StrokeThickness = shapeProperty.SlotProperties.AnnotationProperties.StrokeThickness;
          line1.X1 = leftStart + 2.0 * shapeProperty.PolygonProperties.ScallingFactor;
          line1.X2 = leftStart + 2.0 * shapeProperty.PolygonProperties.ScallingFactor;
          Line line2 = line1;
          double num2 = 2.0 * shapeProperty.PolygonProperties.ScallingFactor;
          margin = shapeProperty.PolygonProperties.Margin;
          double top2 = margin.Top;
          double num3 = num2 + top2 + 6.0;
          line2.Y1 = num3;
          Line line3 = line1;
          double num4 = 4.0 * shapeProperty.PolygonProperties.ScallingFactor;
          margin = shapeProperty.PolygonProperties.Margin;
          double top3 = margin.Top;
          double num5 = num4 + top3 - 7.0;
          line3.Y2 = num5;
          if (shapeProperty.SlotProperties.AnnotationProperties.AliasedON)
            line1.SetValue(RenderOptions.EdgeModeProperty, (object) EdgeMode.Aliased);
          Panel.SetZIndex((UIElement) line1, 6);
          uiElementList.Add((UIElement) line1);
        }
        if (cutSeqShape.ShapeSlot.Properties.AnnotationProperties.IsEnabled)
        {
          Line line1 = new Line();
          line1.Stroke = (Brush) solidColorBrush3;
          line1.StrokeThickness = shapeProperty.SlotProperties.AnnotationProperties.StrokeThickness;
          line1.X1 = left + shapeProperty.SlotProperties.StrokeThickness;
          line1.X2 = left + shapeProperty.SlotProperties.StrokeThickness;
          Line line2 = line1;
          double num2 = 2.0 * shapeProperty.PolygonProperties.ScallingFactor;
          margin = shapeProperty.PolygonProperties.Margin;
          double top2 = margin.Top;
          double num3 = num2 + top2 + 6.0;
          line2.Y1 = num3;
          Line line3 = line1;
          double num4 = 4.0 * shapeProperty.PolygonProperties.ScallingFactor;
          margin = shapeProperty.PolygonProperties.Margin;
          double top3 = margin.Top;
          double num5 = num4 + top3 - 7.0;
          line3.Y2 = num5;
          if (shapeProperty.SlotProperties.AnnotationProperties.AliasedON)
            line1.SetValue(RenderOptions.EdgeModeProperty, (object) EdgeMode.Aliased);
          Panel.SetZIndex((UIElement) line1, 6);
          uiElementList.Add((UIElement) line1);
          Line line4 = new Line();
          line4.Stroke = (Brush) solidColorBrush3;
          line4.StrokeThickness = shapeProperty.SlotProperties.AnnotationProperties.StrokeThickness;
          line4.X1 = left + (double) shapeProperty.SlotProperties.Width;
          line4.X2 = left + (double) shapeProperty.SlotProperties.Width;
          Line line5 = line4;
          double num6 = 2.0 * shapeProperty.PolygonProperties.ScallingFactor;
          margin = shapeProperty.PolygonProperties.Margin;
          double top4 = margin.Top;
          double num7 = num6 + top4 + 6.0;
          line5.Y1 = num7;
          Line line6 = line4;
          double num8 = 4.0 * shapeProperty.PolygonProperties.ScallingFactor;
          margin = shapeProperty.PolygonProperties.Margin;
          double top5 = margin.Top;
          double num9 = num8 + top5 - 7.0;
          line6.Y2 = num9;
          if (shapeProperty.SlotProperties.AnnotationProperties.AliasedON)
            line4.SetValue(RenderOptions.EdgeModeProperty, (object) EdgeMode.Aliased);
          Panel.SetZIndex((UIElement) line4, 6);
          uiElementList.Add((UIElement) line4);
        }
        Rectangle rectangle1 = new Rectangle();
        rectangle1.Width = (double) shapeProperty.SlotProperties.Width;
        rectangle1.Height = (double) shapeProperty.SlotProperties.Height;
        rectangle1.Fill = (Brush) solidColorBrush2;
        rectangle1.Stroke = (Brush) solidColorBrush1;
        rectangle1.StrokeThickness = shapeProperty.SlotProperties.StrokeThickness;
        rectangle1.Margin = new Thickness(left, top1, 0.0, 0.0);
        rectangle1.RadiusX = ((double) shapeProperty.SlotProperties.Height + 2.0 * shapeProperty.SlotProperties.StrokeThickness) / 2.0;
        rectangle1.RadiusY = ((double) shapeProperty.SlotProperties.Height + 2.0 * shapeProperty.SlotProperties.StrokeThickness) / 2.0;
        if (shapeProperty.SlotProperties.AliasedON)
          rectangle1.SetValue(RenderOptions.EdgeModeProperty, (object) EdgeMode.Aliased);
        if (cutSeqShape.ShapeSlot.NumberOfSlotes > 1)
        {
          rectangle1.Margin = new Thickness(left, top1 - shapeProperty.SlotProperties.VerticalDistance / 2.0, 0.0, 0.0);
          Rectangle rectangle2 = new Rectangle();
          rectangle2.Width = (double) shapeProperty.SlotProperties.Width;
          rectangle2.Height = (double) shapeProperty.SlotProperties.Height;
          rectangle2.Fill = (Brush) solidColorBrush2;
          rectangle2.Stroke = (Brush) solidColorBrush1;
          rectangle2.StrokeThickness = shapeProperty.SlotProperties.StrokeThickness;
          rectangle2.Margin = new Thickness(left, top1 + shapeProperty.SlotProperties.VerticalDistance / 2.0, 0.0, 0.0);
          rectangle2.RadiusX = ((double) shapeProperty.SlotProperties.Height + 2.0 * shapeProperty.SlotProperties.StrokeThickness) / 2.0;
          rectangle2.RadiusY = ((double) shapeProperty.SlotProperties.Height + 2.0 * shapeProperty.SlotProperties.StrokeThickness) / 2.0;
          if (shapeProperty.SlotProperties.AliasedON)
            rectangle2.SetValue(RenderOptions.EdgeModeProperty, (object) EdgeMode.Aliased);
          uiElementList.Add((UIElement) rectangle2);
        }
        uiElementList.Add((UIElement) rectangle1);
        if (cutSeqShape.ShapeSlot.MeasuringType != EMeasuringType.Simetric && cutSeqShape.ShapeSlot.Properties.ArrowLineProperties.IsEnabled)
        {
          Line line1 = new Line();
          line1.Stroke = (Brush) solidColorBrush4;
          line1.StrokeThickness = shapeProperty.SlotProperties.ArrowLineProperties.StrokeThickness;
          line1.X1 = leftStart + 2.0 * shapeProperty.PolygonProperties.ScallingFactor + shapeProperty.SlotProperties.AnnotationProperties.StrokeThickness / 2.0;
          line1.X2 = left + shapeProperty.SlotProperties.StrokeThickness - shapeProperty.SlotProperties.AnnotationProperties.StrokeThickness / 2.0;
          Line line2 = line1;
          double num2 = 4.0 * shapeProperty.PolygonProperties.ScallingFactor;
          margin = shapeProperty.PolygonProperties.Margin;
          double top2 = margin.Top;
          double num3 = num2 + top2 - 5.0 - shapeProperty.SlotProperties.ArrowLineProperties.StrokeThickness / 2.0 - 2.0;
          line2.Y1 = num3;
          Line line3 = line1;
          double num4 = 4.0 * shapeProperty.PolygonProperties.ScallingFactor;
          margin = shapeProperty.PolygonProperties.Margin;
          double top3 = margin.Top;
          double num5 = num4 + top3 - 5.0 - shapeProperty.SlotProperties.ArrowLineProperties.StrokeThickness / 2.0 - 2.0;
          line3.Y2 = num5;
          if (shapeProperty.SlotProperties.ArrowLineProperties.AliasedON)
            line1.SetValue(RenderOptions.EdgeModeProperty, (object) EdgeMode.Aliased);
          Panel.SetZIndex((UIElement) line1, 6);
          uiElementList.Add((UIElement) line1);
        }
        if (cutSeqShape.ShapeSlot.Properties.ArrowLineProperties.IsEnabled)
        {
          Line line1 = new Line();
          line1.Stroke = (Brush) solidColorBrush5;
          line1.StrokeThickness = shapeProperty.SlotProperties.ArrowLineProperties.StrokeThickness;
          line1.X1 = left + shapeProperty.SlotProperties.AnnotationProperties.StrokeThickness / 2.0;
          line1.X2 = left + rectangle1.Width - shapeProperty.SlotProperties.AnnotationProperties.StrokeThickness / 2.0;
          Line line2 = line1;
          double num2 = 4.0 * shapeProperty.PolygonProperties.ScallingFactor;
          margin = shapeProperty.PolygonProperties.Margin;
          double top2 = margin.Top;
          double num3 = num2 + top2 - 5.0 - shapeProperty.SlotProperties.ArrowLineProperties.StrokeThickness / 2.0 - 2.0;
          line2.Y1 = num3;
          Line line3 = line1;
          double num4 = 4.0 * shapeProperty.PolygonProperties.ScallingFactor;
          margin = shapeProperty.PolygonProperties.Margin;
          double top3 = margin.Top;
          double num5 = num4 + top3 - 5.0 - shapeProperty.SlotProperties.ArrowLineProperties.StrokeThickness / 2.0 - 2.0;
          line3.Y2 = num5;
          if (shapeProperty.SlotProperties.ArrowLineProperties.AliasedON)
            line1.SetValue(RenderOptions.EdgeModeProperty, (object) EdgeMode.Aliased);
          Panel.SetZIndex((UIElement) line1, 6);
          uiElementList.Add((UIElement) line1);
        }
        Label label = new Label();
        if (cutSeqShape.ShapeSlot.NumberOfSlotes > 2 && cutSeqShape.ShapeSlot.Properties.LabelProperties.IsEnabled)
        {
          label.Content = (object) ("+" + (cutSeqShape.ShapeSlot.NumberOfSlotes - 2).ToString());
          label.Margin = new Thickness(left - 18.0, top1 - 20.0, 0.0, 0.0);
          label.FontSize = 10.0;
          label.Foreground = (Brush) solidColorBrush6;
          label.Width = 50.0;
          label.Height = 30.0;
          uiElementList.Add((UIElement) label);
          label.Visibility = Visibility.Visible;
        }
        else
          label.Visibility = Visibility.Hidden;
      }
      return uiElementList;
    }

    private IList<UIElement> getCenters(CutSeqShape cutSeqShape, double leftStart, ShapeProperties shapeProperty)
    {
      IList<UIElement> uiElementList = (IList<UIElement>) new List<UIElement>();
      SolidColorBrush solidColorBrush1 = new SolidColorBrush();
      solidColorBrush1.Color = shapeProperty.CenterProperties.AnnotationProperties.LineColor;
      SolidColorBrush solidColorBrush2 = new SolidColorBrush();
      solidColorBrush2.Color = shapeProperty.CenterProperties.ArrowLineProperties.Line1Color;
      SolidColorBrush solidColorBrush3 = new SolidColorBrush();
      solidColorBrush3.Color = shapeProperty.CenterProperties.ArrowLineProperties.Line2Color;
      SolidColorBrush solidColorBrush4 = new SolidColorBrush();
      solidColorBrush4.Color = shapeProperty.CenterProperties.ArrowLineProperties.Line3Color;
      SolidColorBrush solidColorBrush5 = new SolidColorBrush();
      solidColorBrush5.Color = shapeProperty.CenterProperties.ArrowLineProperties.Line4Color;
      double num1 = cutSeqShape.Parts.Count < 3 || cutSeqShape.ShapeCenter.MeasuringType == EMeasuringType.Simetric && cutSeqShape.Parts.Count == 3 ? shapeProperty.PolygonProperties.ScallingFactor * (8.0 + 2.0 * shapeProperty.PolygonProperties.DistanceBetweenParts) : shapeProperty.PolygonProperties.ScallingFactor * (4.0 + shapeProperty.PolygonProperties.DistanceBetweenParts);
      for (int index = 0; index < cutSeqShape.Parts.Count; ++index)
      {
        if (cutSeqShape.ShapeCenter.Properties.AnnotationProperties.IsEnabled && index < 5 && (cutSeqShape.Parts.Count != 3 || index <= 1 || cutSeqShape.ShapeCenter.MeasuringType != EMeasuringType.Simetric))
        {
          Line line1 = new Line();
          line1.Stroke = (Brush) solidColorBrush1;
          line1.StrokeThickness = shapeProperty.CenterProperties.AnnotationProperties.StrokeThickness;
          line1.X1 = leftStart + 2.0 * shapeProperty.PolygonProperties.ScallingFactor + (double) index * num1;
          line1.X2 = leftStart + 2.0 * shapeProperty.PolygonProperties.ScallingFactor + (double) index * num1;
          line1.Y1 = 2.0 * shapeProperty.PolygonProperties.ScallingFactor + shapeProperty.PolygonProperties.Margin.Top + 6.0;
          line1.Y2 = 4.0 * shapeProperty.PolygonProperties.ScallingFactor + shapeProperty.PolygonProperties.Margin.Top - 7.0;
          if (shapeProperty.CenterProperties.AnnotationProperties.AliasedON)
            line1.SetValue(RenderOptions.EdgeModeProperty, (object) EdgeMode.Aliased);
          Panel.SetZIndex((UIElement) line1, 7);
          uiElementList.Add((UIElement) line1);
          if (index == 0 && (cutSeqShape.Parts.First<CutSeqShapePart>().EShapePart == EToolCutSequence.CBl || cutSeqShape.Parts.First<CutSeqShapePart>().EShapePart == EToolCutSequence.CFl))
          {
            Line line2 = new Line();
            line2.Stroke = (Brush) solidColorBrush1;
            line2.StrokeThickness = shapeProperty.CenterProperties.AnnotationProperties.StrokeThickness;
            line2.X1 = leftStart + 2.0 * shapeProperty.PolygonProperties.ScallingFactor + (double) index * num1 - shapeProperty.PolygonProperties.ScallingFactor;
            line2.X2 = leftStart + 2.0 * shapeProperty.PolygonProperties.ScallingFactor + (double) index * num1 - shapeProperty.PolygonProperties.ScallingFactor;
            line2.Y1 = 2.0 * shapeProperty.PolygonProperties.ScallingFactor + shapeProperty.PolygonProperties.Margin.Top + 6.0;
            line2.Y2 = 4.0 * shapeProperty.PolygonProperties.ScallingFactor + shapeProperty.PolygonProperties.Margin.Top - 7.0;
            if (shapeProperty.CenterProperties.AnnotationProperties.AliasedON)
              line2.SetValue(RenderOptions.EdgeModeProperty, (object) EdgeMode.Aliased);
            Panel.SetZIndex((UIElement) line2, 7);
            uiElementList.Add((UIElement) line2);
            Line line3 = new Line();
            line3.Stroke = (Brush) solidColorBrush4;
            line3.StrokeThickness = shapeProperty.CenterProperties.ArrowLineProperties.StrokeThickness;
            line3.X1 = leftStart + 2.0 * shapeProperty.PolygonProperties.ScallingFactor + shapeProperty.CenterProperties.AnnotationProperties.StrokeThickness / 2.0 + (double) index * num1 - shapeProperty.PolygonProperties.ScallingFactor;
            line3.X2 = leftStart + 2.0 * shapeProperty.PolygonProperties.ScallingFactor - shapeProperty.CenterProperties.AnnotationProperties.StrokeThickness / 2.0 + (double) index * num1;
            line3.Y1 = 4.0 * shapeProperty.PolygonProperties.ScallingFactor + shapeProperty.PolygonProperties.Margin.Top - shapeProperty.CenterProperties.ArrowLineProperties.StrokeThickness / 2.0 - 7.0;
            line3.Y2 = 4.0 * shapeProperty.PolygonProperties.ScallingFactor + shapeProperty.PolygonProperties.Margin.Top - shapeProperty.CenterProperties.ArrowLineProperties.StrokeThickness / 2.0 - 7.0;
            if (shapeProperty.CenterProperties.ArrowLineProperties.AliasedON)
              line3.SetValue(RenderOptions.EdgeModeProperty, (object) EdgeMode.Aliased);
            Panel.SetZIndex((UIElement) line3, 6);
            uiElementList.Add((UIElement) line3);
          }
          if (index == cutSeqShape.Parts.Count - 1 && (cutSeqShape.Parts[cutSeqShape.Parts.Last<CutSeqShapePart>().SourceIndex].EShapePart == EToolCutSequence.CBr || cutSeqShape.Parts[cutSeqShape.Parts.Last<CutSeqShapePart>().SourceIndex].EShapePart == EToolCutSequence.CFr))
          {
            Line line2 = new Line();
            line2.Stroke = (Brush) solidColorBrush1;
            line2.StrokeThickness = shapeProperty.CenterProperties.AnnotationProperties.StrokeThickness;
            line2.X1 = leftStart + 2.0 * shapeProperty.PolygonProperties.ScallingFactor + (double) index * num1 + shapeProperty.PolygonProperties.ScallingFactor;
            line2.X2 = leftStart + 2.0 * shapeProperty.PolygonProperties.ScallingFactor + (double) index * num1 + shapeProperty.PolygonProperties.ScallingFactor;
            line2.Y1 = 2.0 * shapeProperty.PolygonProperties.ScallingFactor + shapeProperty.PolygonProperties.Margin.Top + 6.0;
            line2.Y2 = 4.0 * shapeProperty.PolygonProperties.ScallingFactor + shapeProperty.PolygonProperties.Margin.Top - 7.0;
            if (shapeProperty.CenterProperties.AnnotationProperties.AliasedON)
              line2.SetValue(RenderOptions.EdgeModeProperty, (object) EdgeMode.Aliased);
            Panel.SetZIndex((UIElement) line2, 7);
            uiElementList.Add((UIElement) line2);
            Line line3 = new Line();
            line3.Stroke = (Brush) solidColorBrush5;
            line3.StrokeThickness = shapeProperty.CenterProperties.ArrowLineProperties.StrokeThickness;
            line3.X1 = leftStart + 2.0 * shapeProperty.PolygonProperties.ScallingFactor + shapeProperty.CenterProperties.AnnotationProperties.StrokeThickness / 2.0 + (double) index * num1;
            line3.X2 = leftStart + 2.0 * shapeProperty.PolygonProperties.ScallingFactor - shapeProperty.CenterProperties.AnnotationProperties.StrokeThickness / 2.0 + (double) index * num1 + shapeProperty.PolygonProperties.ScallingFactor;
            line3.Y1 = 4.0 * shapeProperty.PolygonProperties.ScallingFactor + shapeProperty.PolygonProperties.Margin.Top - shapeProperty.CenterProperties.ArrowLineProperties.StrokeThickness / 2.0 - 7.0;
            line3.Y2 = 4.0 * shapeProperty.PolygonProperties.ScallingFactor + shapeProperty.PolygonProperties.Margin.Top - shapeProperty.CenterProperties.ArrowLineProperties.StrokeThickness / 2.0 - 7.0;
            if (shapeProperty.CenterProperties.ArrowLineProperties.AliasedON)
              line3.SetValue(RenderOptions.EdgeModeProperty, (object) EdgeMode.Aliased);
            Panel.SetZIndex((UIElement) line3, 6);
            uiElementList.Add((UIElement) line3);
          }
        }
      }
      for (int index = 0; index < cutSeqShape.Parts.Count - 1; ++index)
      {
        if (cutSeqShape.ShapeCenter.Properties.ArrowLineProperties.IsEnabled && index < 4 && (cutSeqShape.Parts.Count != 3 || index <= 0 || cutSeqShape.ShapeCenter.MeasuringType != EMeasuringType.Simetric))
        {
          Line line1 = new Line();
          switch (index)
          {
            case 0:
              line1.Stroke = (Brush) solidColorBrush2;
              break;
            case 1:
              line1.Stroke = (Brush) solidColorBrush3;
              break;
            case 2:
              line1.Stroke = (Brush) solidColorBrush4;
              break;
            default:
              line1.Stroke = (Brush) solidColorBrush5;
              break;
          }
          line1.StrokeThickness = shapeProperty.CenterProperties.ArrowLineProperties.StrokeThickness;
          line1.X1 = leftStart + 2.0 * shapeProperty.PolygonProperties.ScallingFactor + shapeProperty.CenterProperties.AnnotationProperties.StrokeThickness / 2.0 + (double) index * num1;
          line1.X2 = leftStart + 2.0 * shapeProperty.PolygonProperties.ScallingFactor - shapeProperty.CenterProperties.AnnotationProperties.StrokeThickness / 2.0 + (double) (index + 1) * num1;
          Line line2 = line1;
          double num2 = 4.0 * shapeProperty.PolygonProperties.ScallingFactor;
          Thickness margin = shapeProperty.PolygonProperties.Margin;
          double top1 = margin.Top;
          double num3 = num2 + top1 - shapeProperty.CenterProperties.ArrowLineProperties.StrokeThickness / 2.0 - 7.0;
          line2.Y1 = num3;
          Line line3 = line1;
          double num4 = 4.0 * shapeProperty.PolygonProperties.ScallingFactor;
          margin = shapeProperty.PolygonProperties.Margin;
          double top2 = margin.Top;
          double num5 = num4 + top2 - shapeProperty.CenterProperties.ArrowLineProperties.StrokeThickness / 2.0 - 7.0;
          line3.Y2 = num5;
          if (shapeProperty.CenterProperties.ArrowLineProperties.AliasedON)
            line1.SetValue(RenderOptions.EdgeModeProperty, (object) EdgeMode.Aliased);
          Panel.SetZIndex((UIElement) line1, 6);
          uiElementList.Add((UIElement) line1);
        }
      }
      return uiElementList;
    }

    private IList<CutSeqShape> getStepLapTC(CutSeqShape cutSeqShape, ShapeProperties shapeProperty)
    {
      IList<CutSeqShape> cutSeqShapeList = (IList<CutSeqShape>) new List<CutSeqShape>();
      CutSeqShape cutSeqShape1 = (CutSeqShape) cutSeqShape.Clone();
      CutSeqShape cutSeqShape2 = (CutSeqShape) cutSeqShape.Clone();
      CutSeqShape cutSeqShape3 = (CutSeqShape) cutSeqShape.Clone();
      for (int index1 = 0; index1 < cutSeqShape.Parts.Count; ++index1)
      {
        switch (cutSeqShape.Parts[index1].ESLType)
        {
          case EStepLapType.Cut90Left:
          case EStepLapType.Cut45Left:
          case EStepLapType.Cut135Left:
          case EStepLapType.CTipLeft:
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S45 && cutSeqShape.TipCatStart)
            {
              this.changeCutSeqShapePartEShapePart(cutSeqShape1.Parts[index1], EToolCutSequence.S45_TCl);
              this.changeCutSeqShapePartEShapePart(cutSeqShape2.Parts[index1], EToolCutSequence.S45_TCl);
              this.changeCutSeqShapePartEShapePart(cutSeqShape3.Parts[index1], EToolCutSequence.S45_TCl);
              continue;
            }
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S135 && cutSeqShape.TipCatStart)
            {
              this.changeCutSeqShapePartEShapePart(cutSeqShape1.Parts[index1], EToolCutSequence.S135_TCl);
              this.changeCutSeqShapePartEShapePart(cutSeqShape2.Parts[index1], EToolCutSequence.S135_TCl);
              this.changeCutSeqShapePartEShapePart(cutSeqShape3.Parts[index1], EToolCutSequence.S135_TCl);
              continue;
            }
            continue;
          case EStepLapType.Cut90Left_Right:
          case EStepLapType.Cut45Left_Right:
          case EStepLapType.Cut135Left_Right:
          case EStepLapType.CTipLeft_Right:
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S45 && cutSeqShape.TipCatStart)
            {
              this.changeCutSeqShapePartEShapePart(cutSeqShape.Parts[index1], EToolCutSequence.S45_TCl);
              this.changeCutSeqShapePartEShapePart(cutSeqShape1.Parts[index1], EToolCutSequence.S45_TCl);
              this.changeCutSeqShapePartEShapePart(cutSeqShape2.Parts[index1], EToolCutSequence.S45_TCl);
              this.changeCutSeqShapePartEShapePart(cutSeqShape3.Parts[index1], EToolCutSequence.S45_TCl);
            }
            else if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S135 && cutSeqShape.TipCatStart)
            {
              this.changeCutSeqShapePartEShapePart(cutSeqShape.Parts[index1], EToolCutSequence.S135_TCl);
              this.changeCutSeqShapePartEShapePart(cutSeqShape1.Parts[index1], EToolCutSequence.S135_TCl);
              this.changeCutSeqShapePartEShapePart(cutSeqShape2.Parts[index1], EToolCutSequence.S135_TCl);
              this.changeCutSeqShapePartEShapePart(cutSeqShape3.Parts[index1], EToolCutSequence.S135_TCl);
            }
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S90 || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S45 || (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S135 || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.Cl) || (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.CFl || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.CBl))
            {
              for (int index2 = 0; index2 < cutSeqShape1.Parts[index1].Points.Count; ++index2)
              {
                cutSeqShape1.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                cutSeqShape3.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
              }
              continue;
            }
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S45_TCl)
            {
              for (int index2 = 0; index2 < cutSeqShape1.Parts[index1].Points.Count; ++index2)
              {
                if (cutSeqShape1.Parts[index1].Points[index2].PointPartIndex == 1)
                {
                  cutSeqShape1.Parts[index1].Points[index2].Y -= shapeProperty.StepLapProperties.Step;
                  cutSeqShape3.Parts[index1].Points[index2].Y += shapeProperty.StepLapProperties.Step;
                }
                else if (cutSeqShape1.Parts[index1].Points[index2].PointPartIndex == 0)
                {
                  cutSeqShape1.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                  cutSeqShape3.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                }
              }
              continue;
            }
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S135_TCl)
            {
              for (int index2 = 0; index2 < cutSeqShape1.Parts[index1].Points.Count; ++index2)
              {
                if (cutSeqShape1.Parts[index1].Points[index2].PointPartIndex == 1)
                {
                  cutSeqShape1.Parts[index1].Points[index2].Y += shapeProperty.StepLapProperties.Step;
                  cutSeqShape3.Parts[index1].Points[index2].Y -= shapeProperty.StepLapProperties.Step;
                }
                else if (cutSeqShape1.Parts[index1].Points[index2].PointPartIndex == 2)
                {
                  cutSeqShape1.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                  cutSeqShape3.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                }
              }
              continue;
            }
            continue;
          case EStepLapType.Cut90Left_Left:
          case EStepLapType.Cut45Left_Left:
          case EStepLapType.Cut135Left_Left:
          case EStepLapType.CTipLeft_Left:
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S45 && cutSeqShape.TipCatStart)
            {
              this.changeCutSeqShapePartEShapePart(cutSeqShape.Parts[index1], EToolCutSequence.S45_TCl);
              this.changeCutSeqShapePartEShapePart(cutSeqShape1.Parts[index1], EToolCutSequence.S45_TCl);
              this.changeCutSeqShapePartEShapePart(cutSeqShape2.Parts[index1], EToolCutSequence.S45_TCl);
              this.changeCutSeqShapePartEShapePart(cutSeqShape3.Parts[index1], EToolCutSequence.S45_TCl);
            }
            else if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S135 && cutSeqShape.TipCatStart)
            {
              this.changeCutSeqShapePartEShapePart(cutSeqShape.Parts[index1], EToolCutSequence.S135_TCl);
              this.changeCutSeqShapePartEShapePart(cutSeqShape1.Parts[index1], EToolCutSequence.S135_TCl);
              this.changeCutSeqShapePartEShapePart(cutSeqShape2.Parts[index1], EToolCutSequence.S135_TCl);
              this.changeCutSeqShapePartEShapePart(cutSeqShape3.Parts[index1], EToolCutSequence.S135_TCl);
            }
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S90 || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S45 || (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S135 || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.Cl) || (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.CFl || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.CBl))
            {
              for (int index2 = 0; index2 < cutSeqShape1.Parts[index1].Points.Count; ++index2)
              {
                cutSeqShape1.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                cutSeqShape3.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
              }
              continue;
            }
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S45_TCl)
            {
              for (int index2 = 0; index2 < cutSeqShape1.Parts[index1].Points.Count; ++index2)
              {
                switch (index2)
                {
                  case 0:
                    cutSeqShape1.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                    break;
                  case 1:
                    cutSeqShape1.Parts[index1].Points[index2].Y += shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].Y -= shapeProperty.StepLapProperties.Step;
                    break;
                }
              }
              continue;
            }
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S135_TCl)
            {
              for (int index2 = 0; index2 < cutSeqShape1.Parts[index1].Points.Count; ++index2)
              {
                switch (index2)
                {
                  case 1:
                    cutSeqShape1.Parts[index1].Points[index2].Y -= shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].Y += shapeProperty.StepLapProperties.Step;
                    break;
                  case 2:
                    cutSeqShape1.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                    break;
                }
              }
              continue;
            }
            continue;
          case EStepLapType.Cut90Right:
          case EStepLapType.Cut45Right:
          case EStepLapType.Cut135Right:
          case EStepLapType.CTipRight:
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S45 && cutSeqShape.TipCatEnd)
            {
              this.changeCutSeqShapePartEShapePart(cutSeqShape1.Parts[index1], EToolCutSequence.S45_TCr);
              this.changeCutSeqShapePartEShapePart(cutSeqShape2.Parts[index1], EToolCutSequence.S45_TCr);
              this.changeCutSeqShapePartEShapePart(cutSeqShape3.Parts[index1], EToolCutSequence.S45_TCr);
              continue;
            }
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S135 && cutSeqShape.TipCatEnd)
            {
              this.changeCutSeqShapePartEShapePart(cutSeqShape1.Parts[index1], EToolCutSequence.S135_TCr);
              this.changeCutSeqShapePartEShapePart(cutSeqShape2.Parts[index1], EToolCutSequence.S135_TCr);
              this.changeCutSeqShapePartEShapePart(cutSeqShape3.Parts[index1], EToolCutSequence.S135_TCr);
              continue;
            }
            continue;
          case EStepLapType.Cut90Right_Left:
          case EStepLapType.Cut45Right_Left:
          case EStepLapType.Cut135Right_Left:
          case EStepLapType.CTipRight_Left:
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S45 && cutSeqShape.TipCatEnd)
            {
              this.changeCutSeqShapePartEShapePart(cutSeqShape.Parts[index1], EToolCutSequence.S45_TCr);
              this.changeCutSeqShapePartEShapePart(cutSeqShape1.Parts[index1], EToolCutSequence.S45_TCr);
              this.changeCutSeqShapePartEShapePart(cutSeqShape2.Parts[index1], EToolCutSequence.S45_TCr);
              this.changeCutSeqShapePartEShapePart(cutSeqShape3.Parts[index1], EToolCutSequence.S45_TCr);
            }
            else if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S135 && cutSeqShape.TipCatEnd)
            {
              this.changeCutSeqShapePartEShapePart(cutSeqShape.Parts[index1], EToolCutSequence.S135_TCr);
              this.changeCutSeqShapePartEShapePart(cutSeqShape1.Parts[index1], EToolCutSequence.S135_TCr);
              this.changeCutSeqShapePartEShapePart(cutSeqShape2.Parts[index1], EToolCutSequence.S135_TCr);
              this.changeCutSeqShapePartEShapePart(cutSeqShape3.Parts[index1], EToolCutSequence.S135_TCr);
            }
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S90 || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S45 || (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S135 || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.Cr) || (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.CFr || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.CBr))
            {
              for (int index2 = 0; index2 < cutSeqShape1.Parts[index1].Points.Count; ++index2)
              {
                cutSeqShape1.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                cutSeqShape3.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
              }
              continue;
            }
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S45_TCr)
            {
              for (int index2 = 0; index2 < cutSeqShape1.Parts[index1].Points.Count; ++index2)
              {
                if (cutSeqShape1.Parts[index1].Points[index2].PointPartIndex == 1)
                {
                  cutSeqShape1.Parts[index1].Points[index2].Y += shapeProperty.StepLapProperties.Step;
                  cutSeqShape3.Parts[index1].Points[index2].Y -= shapeProperty.StepLapProperties.Step;
                }
                else if (cutSeqShape1.Parts[index1].Points[index2].PointPartIndex == 2)
                {
                  cutSeqShape1.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                  cutSeqShape3.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                }
              }
              continue;
            }
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S135_TCr)
            {
              for (int index2 = 0; index2 < cutSeqShape1.Parts[index1].Points.Count; ++index2)
              {
                if (cutSeqShape1.Parts[index1].Points[index2].PointPartIndex == 1)
                {
                  cutSeqShape1.Parts[index1].Points[index2].Y -= shapeProperty.StepLapProperties.Step;
                  cutSeqShape3.Parts[index1].Points[index2].Y += shapeProperty.StepLapProperties.Step;
                }
                else if (cutSeqShape1.Parts[index1].Points[index2].PointPartIndex == 0)
                {
                  cutSeqShape1.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                  cutSeqShape3.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                }
              }
              continue;
            }
            continue;
          case EStepLapType.Cut90Right_Right:
          case EStepLapType.Cut45Right_Right:
          case EStepLapType.Cut135Right_Right:
          case EStepLapType.CTipRight_Right:
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S45 && cutSeqShape.TipCatEnd)
            {
              this.changeCutSeqShapePartEShapePart(cutSeqShape.Parts[index1], EToolCutSequence.S45_TCr);
              this.changeCutSeqShapePartEShapePart(cutSeqShape1.Parts[index1], EToolCutSequence.S45_TCr);
              this.changeCutSeqShapePartEShapePart(cutSeqShape2.Parts[index1], EToolCutSequence.S45_TCr);
              this.changeCutSeqShapePartEShapePart(cutSeqShape3.Parts[index1], EToolCutSequence.S45_TCr);
            }
            else if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S135 && cutSeqShape.TipCatEnd)
            {
              this.changeCutSeqShapePartEShapePart(cutSeqShape.Parts[index1], EToolCutSequence.S135_TCr);
              this.changeCutSeqShapePartEShapePart(cutSeqShape1.Parts[index1], EToolCutSequence.S135_TCr);
              this.changeCutSeqShapePartEShapePart(cutSeqShape2.Parts[index1], EToolCutSequence.S135_TCr);
              this.changeCutSeqShapePartEShapePart(cutSeqShape3.Parts[index1], EToolCutSequence.S135_TCr);
            }
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S90 || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S45 || (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S135 || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.Cr) || (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.CFr || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.CBr))
            {
              for (int index2 = 0; index2 < cutSeqShape1.Parts[index1].Points.Count; ++index2)
              {
                cutSeqShape1.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                cutSeqShape3.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
              }
              continue;
            }
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S45_TCr)
            {
              for (int index2 = 0; index2 < cutSeqShape1.Parts[index1].Points.Count; ++index2)
              {
                if (cutSeqShape1.Parts[index1].Points[index2].PointPartIndex == 1)
                {
                  cutSeqShape1.Parts[index1].Points[index2].Y -= shapeProperty.StepLapProperties.Step;
                  cutSeqShape3.Parts[index1].Points[index2].Y += shapeProperty.StepLapProperties.Step;
                }
                else if (cutSeqShape1.Parts[index1].Points[index2].PointPartIndex == 2)
                {
                  cutSeqShape1.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                  cutSeqShape3.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                }
              }
              continue;
            }
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.S135_TCr)
            {
              for (int index2 = 0; index2 < cutSeqShape1.Parts[index1].Points.Count; ++index2)
              {
                if (cutSeqShape1.Parts[index1].Points[index2].PointPartIndex == 1)
                {
                  cutSeqShape1.Parts[index1].Points[index2].Y += shapeProperty.StepLapProperties.Step;
                  cutSeqShape3.Parts[index1].Points[index2].Y -= shapeProperty.StepLapProperties.Step;
                }
                else if (cutSeqShape1.Parts[index1].Points[index2].PointPartIndex == 0)
                {
                  cutSeqShape1.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                  cutSeqShape3.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                }
              }
              continue;
            }
            continue;
          case EStepLapType.CTipLeft_Up:
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.Cl || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.CBl || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.CFl)
            {
              for (int index2 = 0; index2 < cutSeqShape1.Parts[index1].Points.Count; ++index2)
              {
                switch (index2)
                {
                  case 0:
                    cutSeqShape1.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                    break;
                  case 1:
                    cutSeqShape1.Parts[index1].Points[index2].Y += shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].Y -= shapeProperty.StepLapProperties.Step;
                    break;
                  case 2:
                    cutSeqShape1.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                    break;
                }
              }
              continue;
            }
            continue;
          case EStepLapType.CTipLeft_Down:
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.Cl || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.CBl || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.CFl)
            {
              for (int index2 = 0; index2 < cutSeqShape1.Parts[index1].Points.Count; ++index2)
              {
                switch (index2)
                {
                  case 0:
                    cutSeqShape1.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                    break;
                  case 1:
                    cutSeqShape1.Parts[index1].Points[index2].Y -= shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].Y += shapeProperty.StepLapProperties.Step;
                    break;
                  case 2:
                    cutSeqShape1.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                    break;
                }
              }
              continue;
            }
            continue;
          case EStepLapType.CTipRight_Up:
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.Cr || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.CBr || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.CFr)
            {
              for (int index2 = 0; index2 < cutSeqShape1.Parts[index1].Points.Count; ++index2)
              {
                switch (index2)
                {
                  case 0:
                    cutSeqShape1.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                    break;
                  case 1:
                    cutSeqShape1.Parts[index1].Points[index2].Y += shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].Y -= shapeProperty.StepLapProperties.Step;
                    break;
                  case 2:
                    cutSeqShape1.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                    break;
                }
              }
              continue;
            }
            continue;
          case EStepLapType.CTipRight_Down:
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.Cr || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.CBr || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.CFr)
            {
              for (int index2 = 0; index2 < cutSeqShape1.Parts[index1].Points.Count; ++index2)
              {
                switch (index2)
                {
                  case 0:
                    cutSeqShape1.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                    break;
                  case 1:
                    cutSeqShape1.Parts[index1].Points[index2].Y -= shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].Y += shapeProperty.StepLapProperties.Step;
                    break;
                  case 2:
                    cutSeqShape1.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                    break;
                }
              }
              continue;
            }
            continue;
          case EStepLapType.VTop:
          case EStepLapType.VBottom:
            continue;
          case EStepLapType.VTop_Right:
          case EStepLapType.VBottom_Right:
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VB || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VF || (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VBS || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VFS))
            {
              for (int index2 = 0; index2 < cutSeqShape1.Parts[index1].Points.Count; ++index2)
              {
                cutSeqShape1.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                cutSeqShape3.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
              }
              continue;
            }
            continue;
          case EStepLapType.VTop_Left:
          case EStepLapType.VBottom_Left:
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VB || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VF || (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VBS || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VFS))
            {
              for (int index2 = 0; index2 < cutSeqShape1.Parts[index1].Points.Count; ++index2)
              {
                cutSeqShape1.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                cutSeqShape3.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
              }
              continue;
            }
            continue;
          case EStepLapType.VTop_Down:
          case EStepLapType.VBottom_Up:
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VB || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VBS)
            {
              for (int index2 = 0; index2 < cutSeqShape1.Parts[index1].Points.Count; ++index2)
              {
                switch (index2)
                {
                  case 0:
                    if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VB)
                    {
                      cutSeqShape2.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                      cutSeqShape1.Parts[index1].Points[index2].X -= 2.0 * shapeProperty.StepLapProperties.Step;
                      break;
                    }
                    cutSeqShape1.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                    break;
                  case 1:
                    if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VB)
                    {
                      cutSeqShape2.Parts[index1].Points[index2].Y -= shapeProperty.StepLapProperties.Step;
                      cutSeqShape1.Parts[index1].Points[index2].Y -= 2.0 * shapeProperty.StepLapProperties.Step;
                      break;
                    }
                    cutSeqShape1.Parts[index1].Points[index2].Y -= shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].Y += shapeProperty.StepLapProperties.Step;
                    break;
                  case 2:
                    if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VB)
                    {
                      cutSeqShape2.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                      cutSeqShape1.Parts[index1].Points[index2].X += 2.0 * shapeProperty.StepLapProperties.Step;
                      break;
                    }
                    cutSeqShape1.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                    break;
                }
              }
            }
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VF || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VFS)
            {
              for (int index2 = 0; index2 < cutSeqShape1.Parts[index1].Points.Count; ++index2)
              {
                switch (index2)
                {
                  case 0:
                    if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VF)
                    {
                      cutSeqShape2.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                      cutSeqShape1.Parts[index1].Points[index2].X += 2.0 * shapeProperty.StepLapProperties.Step;
                      break;
                    }
                    cutSeqShape1.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                    break;
                  case 1:
                    if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VF)
                    {
                      cutSeqShape2.Parts[index1].Points[index2].Y += shapeProperty.StepLapProperties.Step;
                      cutSeqShape1.Parts[index1].Points[index2].Y += 2.0 * shapeProperty.StepLapProperties.Step;
                      break;
                    }
                    cutSeqShape1.Parts[index1].Points[index2].Y += shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].Y -= shapeProperty.StepLapProperties.Step;
                    break;
                  case 2:
                    if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VF)
                    {
                      cutSeqShape2.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                      cutSeqShape1.Parts[index1].Points[index2].X -= 2.0 * shapeProperty.StepLapProperties.Step;
                      break;
                    }
                    cutSeqShape1.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                    break;
                }
              }
              continue;
            }
            continue;
          case EStepLapType.VTop_Up:
          case EStepLapType.VBottom_Down:
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VB || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VBS)
            {
              for (int index2 = 0; index2 < cutSeqShape1.Parts[index1].Points.Count; ++index2)
              {
                switch (index2)
                {
                  case 0:
                    if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VB)
                    {
                      cutSeqShape2.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                      cutSeqShape3.Parts[index1].Points[index2].X -= 2.0 * shapeProperty.StepLapProperties.Step;
                      break;
                    }
                    cutSeqShape1.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                    break;
                  case 1:
                    if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VB)
                    {
                      cutSeqShape2.Parts[index1].Points[index2].Y -= shapeProperty.StepLapProperties.Step;
                      cutSeqShape3.Parts[index1].Points[index2].Y -= 2.0 * shapeProperty.StepLapProperties.Step;
                      break;
                    }
                    cutSeqShape1.Parts[index1].Points[index2].Y += shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].Y -= shapeProperty.StepLapProperties.Step;
                    break;
                  case 2:
                    if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VB)
                    {
                      cutSeqShape2.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                      cutSeqShape3.Parts[index1].Points[index2].X += 2.0 * shapeProperty.StepLapProperties.Step;
                      break;
                    }
                    cutSeqShape1.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                    break;
                }
              }
            }
            if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VF || cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VFS)
            {
              for (int index2 = 0; index2 < cutSeqShape1.Parts[index1].Points.Count; ++index2)
              {
                switch (index2)
                {
                  case 0:
                    if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VF)
                    {
                      cutSeqShape2.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                      cutSeqShape3.Parts[index1].Points[index2].X += 2.0 * shapeProperty.StepLapProperties.Step;
                      break;
                    }
                    cutSeqShape1.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                    break;
                  case 1:
                    if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VF)
                    {
                      cutSeqShape2.Parts[index1].Points[index2].Y += shapeProperty.StepLapProperties.Step;
                      cutSeqShape3.Parts[index1].Points[index2].Y += 2.0 * shapeProperty.StepLapProperties.Step;
                      break;
                    }
                    cutSeqShape1.Parts[index1].Points[index2].Y -= shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].Y += shapeProperty.StepLapProperties.Step;
                    break;
                  case 2:
                    if (cutSeqShape.Parts[index1].EShapePart == EToolCutSequence.VF)
                    {
                      cutSeqShape2.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                      cutSeqShape3.Parts[index1].Points[index2].X -= 2.0 * shapeProperty.StepLapProperties.Step;
                      break;
                    }
                    cutSeqShape1.Parts[index1].Points[index2].X += shapeProperty.StepLapProperties.Step;
                    cutSeqShape3.Parts[index1].Points[index2].X -= shapeProperty.StepLapProperties.Step;
                    break;
                }
              }
              continue;
            }
            continue;
          default:
            int num = (int) MessageBox.Show("Error reading XML file");
            continue;
        }
      }
      cutSeqShapeList.Add(cutSeqShape1);
      cutSeqShapeList.Add(cutSeqShape2);
      cutSeqShapeList.Add(cutSeqShape3);
      return cutSeqShapeList;
    }

    public IList<CutSeqShape> GetCutSeqShapes(CutSequence cutSeq, ShapeProperties shapeProperty)
    {
      this.cutSeqName = cutSeq.Name;
      return this.GetCutSeqShapes((IList<Shape>) cutSeq.Shapes, shapeProperty);
    }

    public IList<CutSeqShape> GetCutSeqShapes(IList<Shape> shapes, ShapeProperties shapeProperty)
    {
      IList<CutSeqShape> cutSeqShapeList = (IList<CutSeqShape>) new List<CutSeqShape>();
      foreach (IList<EToolCutSequence> etoolCutSequenceList in (IEnumerable<IList<EToolCutSequence>>) this.convertCutSequence(shapes))
      {
        CutSeqShape cutSeqShape = new CutSeqShape();
        if (etoolCutSequenceList.Count > 0 && ProjectConstants.CutSequenceModel.ContainsKey(etoolCutSequenceList[0]) && ProjectConstants.CutSequenceModel[etoolCutSequenceList[0]].Count == 3)
        {
          IList<PointXY> pointXyList = ProjectConstants.CutSequenceModel[etoolCutSequenceList[0]];
          cutSeqShape.Parts.Add(new CutSeqShapePart()
          {
            EShapePart = etoolCutSequenceList[0],
            Points = (IList<PointXY>) new List<PointXY>()
            {
              (PointXY) pointXyList[0].Clone(),
              (PointXY) pointXyList[1].Clone(),
              (PointXY) pointXyList[2].Clone()
            },
            ESLType = EStepLapType.Cut90Left,
            SourceIndex = 0
          });
          List<int> intList = new List<int>();
          intList.Add(0);
          for (int index1 = 0; index1 < etoolCutSequenceList.Count; ++index1)
          {
            double num1 = double.MaxValue;
            double num2 = double.MaxValue;
            int index2 = 0;
            int index3 = 0;
            for (int index4 = 0; index4 < etoolCutSequenceList.Count; ++index4)
            {
              double num3 = etoolCutSequenceList.Count != 2 ? (double) index4 * (shapeProperty.PolygonProperties.DistanceBetweenParts + 4.0) : (double) (index4 * 2) * (shapeProperty.PolygonProperties.DistanceBetweenParts + 4.0);
              if (cutSeqShape.Parts.Last<CutSeqShapePart>().Points.Last<PointXY>().Y == ProjectConstants.CutSequenceModel[etoolCutSequenceList[index4]][0].Y)
              {
                double num4 = Math.Abs(cutSeqShape.Parts.Last<CutSeqShapePart>().Points.Last<PointXY>().X - (ProjectConstants.CutSequenceModel[etoolCutSequenceList[index4]][0].X + num3));
                if (num4 < num1 && !intList.Contains(index4))
                {
                  num1 = num4;
                  index2 = index4;
                }
              }
              if (cutSeqShape.Parts.Last<CutSeqShapePart>().Points.Last<PointXY>().Y == ProjectConstants.CutSequenceModel[etoolCutSequenceList[index4]][2].Y)
              {
                double num4 = Math.Abs(cutSeqShape.Parts.Last<CutSeqShapePart>().Points.Last<PointXY>().X - (ProjectConstants.CutSequenceModel[etoolCutSequenceList[index4]][2].X + num3));
                if (num4 < num2 && !intList.Contains(index4))
                {
                  num2 = num4;
                  index3 = index4;
                }
              }
            }
            if (index2 > 0 && num1 <= num2)
            {
              double num3 = etoolCutSequenceList.Count != 2 ? (shapeProperty.PolygonProperties.DistanceBetweenParts + 4.0) * (double) index2 : 2.0 * (shapeProperty.PolygonProperties.DistanceBetweenParts + 4.0) * (double) index2;
              PointXY pointXy1 = (PointXY) ProjectConstants.CutSequenceModel[etoolCutSequenceList[index2]][0].Clone();
              pointXy1.X += num3;
              PointXY pointXy2 = (PointXY) ProjectConstants.CutSequenceModel[etoolCutSequenceList[index2]][1].Clone();
              pointXy2.X += num3;
              PointXY pointXy3 = (PointXY) ProjectConstants.CutSequenceModel[etoolCutSequenceList[index2]][2].Clone();
              pointXy3.X += num3;
              cutSeqShape.Parts.Add(new CutSeqShapePart()
              {
                EShapePart = etoolCutSequenceList[index2],
                Points = (IList<PointXY>) new List<PointXY>()
                {
                  pointXy1,
                  pointXy2,
                  pointXy3
                },
                ESLType = index2 < etoolCutSequenceList.Count - 1 ? EStepLapType.VTop : EStepLapType.Cut90Right,
                SourceIndex = index2
              });
              intList.Add(index2);
            }
            if (index3 > 0 && num2 < num1)
            {
              double num3 = etoolCutSequenceList.Count != 2 ? (shapeProperty.PolygonProperties.DistanceBetweenParts + 4.0) * (double) index3 : 2.0 * (shapeProperty.PolygonProperties.DistanceBetweenParts + 4.0) * (double) index3;
              PointXY pointXy1 = (PointXY) ProjectConstants.CutSequenceModel[etoolCutSequenceList[index3]][2].Clone();
              pointXy1.X += num3;
              PointXY pointXy2 = (PointXY) ProjectConstants.CutSequenceModel[etoolCutSequenceList[index3]][1].Clone();
              pointXy2.X += num3;
              PointXY pointXy3 = (PointXY) ProjectConstants.CutSequenceModel[etoolCutSequenceList[index3]][0].Clone();
              pointXy3.X += num3;
              cutSeqShape.Parts.Add(new CutSeqShapePart()
              {
                EShapePart = etoolCutSequenceList[index3],
                Points = (IList<PointXY>) new List<PointXY>()
                {
                  pointXy1,
                  pointXy2,
                  pointXy3
                },
                ESLType = index3 < etoolCutSequenceList.Count - 1 ? EStepLapType.VTop : EStepLapType.Cut90Right,
                SourceIndex = index3
              });
              intList.Add(index3);
            }
          }
        }
        cutSeqShapeList.Add(cutSeqShape);
      }
      return cutSeqShapeList;
    }

    public List<UIElement> GetCutSeqDrawData(IList<CutSeqShape> cutSeqShapes, ShapeProperties shapeProperty)
    {
      List<UIElement> uiElementList = new List<UIElement>();
      foreach (CutSeqShape cutSeqShape in (IEnumerable<CutSeqShape>) cutSeqShapes)
      {
        IList<CutSeqShape> stepLapTc = this.getStepLapTC(cutSeqShape, shapeProperty);
        IList<Polygon> source = (IList<Polygon>) new List<Polygon>();
        for (int layerPosition = 0; layerPosition < stepLapTc.Count; ++layerPosition)
        {
          Polygon shapePolygon = this.getShapePolygon(stepLapTc[layerPosition], layerPosition, shapeProperty);
          source.Add(shapePolygon);
        }
        foreach (Polygon polygon in (IEnumerable<Polygon>) source)
          uiElementList.Add((UIElement) polygon);
        if (cutSeqShape.ShapeHole != null)
          uiElementList.AddRange((IEnumerable<UIElement>) this.getHoles(cutSeqShape, source.Last<Polygon>().Margin.Left, shapeProperty));
        if (cutSeqShape.ShapeSlot != null)
          uiElementList.AddRange((IEnumerable<UIElement>) this.getSlots(cutSeqShape, source.Last<Polygon>().Margin.Left, shapeProperty));
        if (cutSeqShape.ShapeCenter != null)
          uiElementList.AddRange((IEnumerable<UIElement>) this.getCenters(cutSeqShape, source.Last<Polygon>().Margin.Left, shapeProperty));
      }
      return uiElementList;
    }
  }
}
