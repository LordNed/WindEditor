using OpenTK;

namespace WindViewer.Editor
{
    public abstract class IRenderable
    {
        public Transform Transform;

        public int VertexCount;
        public int IndexCount;
        public Matrix4 ModelMatrix = Matrix4.Identity;
        public Matrix4 ViewProjectionMatrix = Matrix4.Identity;
        public Matrix4 ModelViewProjectionMatrix = Matrix4.Identity;

        public abstract Vector3[] GetVerts();
        public abstract int[] GetIndices(int offset = 0);
        public abstract void CalculateModelMatrix();
        public abstract Vector3[] GetColorData();

        public abstract void Render();
    }
}
