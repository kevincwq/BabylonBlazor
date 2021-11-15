namespace BabylonBlazor.Client.Game;

using System.Collections.Generic;
using System.Text.Json.Serialization;
using BABYLON;
using EventHorizon.Blazor.Interop;

[JsonConverter(typeof(CachedEntityConverter<Player>))]
public class Player : TransformNode
{
    private static readonly Vector3 ORIGINAL_TILT = new(0.5934119456780721m, 0, 0);

    private readonly Scene _scene;
    private readonly Mesh _mesh;
    private TransformNode _cameraRoot;
    private TransformNode _yTilt;
    private UniversalCamera _camera;

    public Player(
        IDictionary<string, Mesh> assets,
        Scene scene,
        ShadowGenerator shadowGenerator
    ) : base("player", scene)
    {
        _scene = scene;
        SetupPlayerCamera();

        _mesh = assets["mesh"];
        _mesh.parent = this;

        shadowGenerator.addShadowCaster(assets["mesh"]);

        // _input = input;
    }

    private void SetupPlayerCamera()
    {
        _cameraRoot = new TransformNode("root")
        {
            position = Vector3.Zero(),
            rotation = new Vector3(0, (decimal)System.Math.PI, 0)
        };

        _yTilt = new TransformNode("y-tilt")
        {
            rotation = ORIGINAL_TILT,
            parent = _cameraRoot
        };

        _camera = new UniversalCamera("cam", new Vector3(0, 0, -30), _scene)
        {
            lockedTarget = _cameraRoot.position,
            fov = 0.47m,
            parent = _yTilt
        };

        _scene.activeCamera = _camera;
        _camera.attachControl(true);
    }
}
