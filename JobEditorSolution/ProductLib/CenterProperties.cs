
namespace ProductLib
{
  public class CenterProperties
  {
    public ArrowLineProperties ArrowLineProperties { get; set; }

    public AnnotationProperties AnnotationProperties { get; set; }

    public CenterProperties()
    {
      this.ArrowLineProperties = new ArrowLineProperties();
      this.AnnotationProperties = new AnnotationProperties();
    }
  }
}
