using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace BPR2_Desktop.Model.Helpers;

public static class ShelfBuilder
{
    /// <summary>
    /// Creates a side of the shelf (left or right).
    /// </summary>
    /// <param name="center">Center point of the side.</param>
    /// <param name="width">Width of the side.</param>
    /// <param name="height">Height of the side.</param>
    /// <param name="depth">Depth of the shelf.</param>
    /// <param name="thickness">Thickness of the shelf side.</param>
    /// <param name="color">Color of the side.</param>
    /// <returns>ModelVisual3D representing the shelf side.</returns>
    public static ModelVisual3D CreateShelfSide(
        Point3D center,
        double width,
        double height,
        double depth,
        double thickness,
        Color color)
    {
        MeshBuilder meshBuilder = new MeshBuilder();

        // The side is a thin box
        meshBuilder.AddBox(center, thickness, height, depth);
        MeshGeometry3D mesh = meshBuilder.ToMesh();

        // Define the material
        Material material = new DiffuseMaterial(new SolidColorBrush(color));

        // Create the GeometryModel3D
        GeometryModel3D sideModel = new GeometryModel3D
        {
            Geometry = mesh,
            Material = material,
            BackMaterial = material
        };

        // Create and return the ModelVisual3D
        ModelVisual3D sideVisual = new ModelVisual3D
        {
            Content = sideModel
        };

        return sideVisual;
    }

    /// <summary>
    /// Creates the back of the shelf.
    /// </summary>
    /// <param name="center">Center point of the back.</param>
    /// <param name="width">Width of the back.</param>
    /// <param name="height">Height of the back.</param>
    /// <param name="depth">Depth of the shelf.</param>
    /// <param name="thickness">Thickness of the back.</param>
    /// <param name="color">Color of the back.</param>
    /// <returns>ModelVisual3D representing the shelf back.</returns>
    public static ModelVisual3D CreateShelfBack(
        Point3D center,
        double width,
        double height,
        double depth,
        double thickness,
        Color color)
    {
        MeshBuilder meshBuilder = new MeshBuilder();

        // The back is a thin box positioned at the rear of the shelf
        meshBuilder.AddBox(center, width, height, thickness);
        MeshGeometry3D mesh = meshBuilder.ToMesh();

        // Define the material
        Material material = new DiffuseMaterial(new SolidColorBrush(color));

        // Create the GeometryModel3D
        GeometryModel3D backModel = new GeometryModel3D
        {
            Geometry = mesh,
            Material = material,
            BackMaterial = material
        };

        // Create and return the ModelVisual3D
        ModelVisual3D backVisual = new ModelVisual3D
        {
            Content = backModel
        };

        return backVisual;
    }

    public static List<ModelVisual3D> CreateShelves(
        int numberOfShelves,
        double distanceBetweenShelves,
        double width,
        double depth,
        double shelveThickness,
        Color color,
        out List<Point3D> shelfPositions)
    {
        shelfPositions = new List<Point3D>();
        List<ModelVisual3D> shelfBoxes = new List<ModelVisual3D>();
        for (int i = 0; i < numberOfShelves; i++)
        {
            double yPosition = i * (distanceBetweenShelves + shelveThickness);
            // Create a fresh mesh builder for each shelf
            var meshBuilder = new MeshBuilder();

            // AddBox requires the center point and size for the box
            var point = new Point3D(width / 2, yPosition, 0);
            meshBuilder.AddBox(
                center: point, // Center point of the box
                xlength: width, // Length in the X direction (Width of the shelf)
                ylength: shelveThickness, // Length in the Y direction (Thickness of the shelf)
                zlength: depth // Length in the Z direction (Depth of the shelf, arbitrarily set to 0.3)
            );
            shelfPositions.Add(point);

            // Convert the mesh to a GeometryModel3D and add materials
            var boxMesh = meshBuilder.ToMesh();
            var boxMaterial = MaterialHelper.CreateMaterial(color);

            var shelfBox = new GeometryModel3D
            {
                Geometry = boxMesh,
                Material = boxMaterial,
                BackMaterial = boxMaterial
            };

            // Add the shelf box as a ModelVisual3D to the Lines collection
            shelfBoxes.Add(new ModelVisual3D { Content = shelfBox });
        }

        return shelfBoxes;
    }
    
    public static List<ModelVisual3D> GetShelfSidesAndBack(Dimensions dimensions, double sideThickness = 0.1, double backThickness = 0.05)
    {
        // Define shelf dimensions
        double shelfWidth = dimensions.Width; // Width along X-axis
        double shelfHeight = dimensions.Height; // Height along Y-axis
        double shelfDepth = dimensions.Depth; // Depth along Z-axis

        // Define colors
        Color sideColor = Colors.Brown;
        Color backColor = Colors.DarkGray;

        // Calculate positions for left side, right side, and back
        // Assuming the shelf is centered at (0,0,0)

        // Left Side Position
        Point3D leftSideCenter = new Point3D(
            x: 0,
            y: shelfHeight / 2,
            z: 0);

        // Right Side Position
        Point3D rightSideCenter = new Point3D(
            x: shelfWidth,
            y: shelfHeight / 2,
            z: 0);

        // Back Position
        Point3D backCenter = new Point3D(
            x: shelfWidth / 2,
            y: shelfHeight / 2,
            z: shelfDepth - shelfDepth / 2);

        // Create left side
        ModelVisual3D leftSide = CreateShelfSide(
            center: leftSideCenter,
            width: sideThickness,
            height: shelfHeight,
            depth: shelfDepth,
            thickness: sideThickness,
            color: sideColor);

        // Create right side
        ModelVisual3D rightSide = CreateShelfSide(
            center: rightSideCenter,
            width: sideThickness,
            height: shelfHeight,
            depth: shelfDepth,
            thickness: sideThickness,
            color: sideColor);

        // Create back
        ModelVisual3D back = CreateShelfBack(
            center: backCenter,
            width: shelfWidth, // Adjust width to fit between sides
            height: shelfHeight,
            depth: shelfDepth,
            thickness: backThickness,
            color: backColor);

        // Collect all parts into a list
        List<ModelVisual3D> shelfParts = new List<ModelVisual3D>
        {
            leftSide,
            rightSide,
            back
        };

        return shelfParts;
    }
}