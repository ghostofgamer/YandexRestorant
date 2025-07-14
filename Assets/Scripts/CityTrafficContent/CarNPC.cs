using UnityEngine;

namespace CityTrafficContent
{
    public class CarNPC : AbstractNPC
    {
        [SerializeField] private Texture[] _textures;
        [SerializeField] private Renderer[] _renderers;

        private Material _material;

        public override void InitUniqueData()
        {
            if (_renderers.Length > 0)
            {
                Texture randomTexture = _textures[Random.Range(0, _textures.Length)];
                
                foreach (var renderer in _renderers)
                {
                    _material = new Material(renderer.material);
                    _material.mainTexture = randomTexture;
                    renderer.material = _material;
                }
            }
        }
    }
}