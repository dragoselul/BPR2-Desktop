using System.IO;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace BPR2_Desktop.Model.Helpers;

public static class ObjExporter3D
{
    public static void Export(Model3D model, string directoryPath, string fileNameWithoutExtension)
    {
        try
        {
            var exporter = new ObjExporter();

            // Ensure the directory exists
            Directory.CreateDirectory(directoryPath + $"{fileNameWithoutExtension}");
            directoryPath += $"{fileNameWithoutExtension}\\";

            // Construct the full path for the OBJ file
            var objFilePath = Path.Combine(directoryPath, $"{fileNameWithoutExtension}.obj");

            // Construct the full path for the MTL file
            string materialsPath = Path.ChangeExtension(objFilePath, ".mtl");

            // Set the materials file path for the exporter
            exporter.MaterialsFile = materialsPath;

            // Validate that the model is a GeometryModel3D
            if (model is not GeometryModel3D geometryModel)
            {
                throw new InvalidOperationException("The provided model is not a GeometryModel3D.");
            }
            ScaleTransform3D scaleTransform = new ScaleTransform3D(0.1, 0.1, 0.1);
            geometryModel.Transform = scaleTransform;
            // Check if the material is set
            if (geometryModel.Material == null)
            {
                throw new InvalidOperationException("The model does not have a material assigned.");
            }

            // Create or overwrite the OBJ file
            
            using var stream = File.Create(objFilePath);
            exporter.Export(geometryModel, stream);

            Console.WriteLine($"Model successfully exported to {objFilePath}");
            Console.WriteLine($"Materials saved to {materialsPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to export model: {ex.Message}");
            throw; // Re-throwing to allow higher layers to handle it if necessary
        }
    }

    public static ModelVisual3D MergeModels(List<ModelVisual3D> models)
    {
        MeshBuilder meshBuilder = new MeshBuilder();

        foreach (var shelfBox in models)
        {
            if (shelfBox.Content is GeometryModel3D geometryModel)
            {
                // Assume the geometry is a MeshGeometry3D
                MeshGeometry3D mesh = geometryModel.Geometry as MeshGeometry3D;
                if (mesh != null)
                {
                    // Extract positions
                    IList<Point3D> positions = mesh.Positions;

                    // Extract triangle indices
                    IList<int> triangleIndices = mesh.TriangleIndices;

                    // Extract normals (ensure they are present)
                    IList<Vector3D> normals = mesh.Normals;
                    if (normals == null || normals.Count == 0)
                    {
                        // Compute normals if missing
                        mesh = meshBuilder.ToMesh();
                        normals = mesh.Normals;
                    }

                    // Extract texture coordinates (if present)
                    IList<Point> textureCoordinates = mesh.TextureCoordinates;
                    // If not using textures, you can pass null or an empty list
                    if (textureCoordinates == null || textureCoordinates.Count == 0)
                    {
                        textureCoordinates = new List<Point>();
                    }

                    // Append to MeshBuilder
                    meshBuilder.Append(positions, triangleIndices, normals, textureCoordinates);
                }
                else
                {
                    throw new InvalidOperationException("Geometry is not a MeshGeometry3D.");
                }
            }
        }

        // Create the combined mesh
        MeshGeometry3D combinedMesh = meshBuilder.ToMesh();

        // Define a single material (or handle multiple materials as needed)
        Material material = new DiffuseMaterial(new SolidColorBrush(Colors.LightGray));

        // Create the GeometryModel3D
        GeometryModel3D combinedGeometryModel = new GeometryModel3D
        {
            Geometry = combinedMesh,
            Material = material,
            BackMaterial = material
        };

        // Create the ModelVisual3D
        ModelVisual3D combinedModelVisual = new ModelVisual3D
        {
            Content = combinedGeometryModel
        };
        return combinedModelVisual;
    }

    
}