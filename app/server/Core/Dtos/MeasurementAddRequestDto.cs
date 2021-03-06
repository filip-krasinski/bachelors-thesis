using System.Collections.Generic;

namespace Core.Dtos;

public class MeasurementAddRequestDto
{
    public string Label { get; set; }
    public string Plane { get; set; }
    public string Type { get; set; }
    public int Slice { get; set; }
    public int[] Vertices { get; set; }
}