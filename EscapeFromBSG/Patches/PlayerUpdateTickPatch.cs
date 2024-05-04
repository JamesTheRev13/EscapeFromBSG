using Aki.Reflection.Patching;
using BepInEx;
using EFT;
using EFT.CameraControl;
using System.Reflection;
using UnityEngine;

namespace EscapeFromBSG.Patches
{
        //////////////////////////////////////////////
       ///             Crude NoClip               ///
      ///            Actively Testing            ///
     ///           Use at your own risk         ///
    //////////////////////////////////////////////
    
    // TODO: Make collision detection configurable
    // TODO: Figure out a proper way to handle collisions - brute forcing your way through walls is not the way to go
    public class PlayerUpdateTickPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(Player).GetMethod(nameof(Player.UpdateTick));

        [PatchPrefix]
        private static void Prefix(Player __instance)
        {
            var player = __instance;

            if (player == null || !player.IsYourPlayer || !Plugin.NoClip.Value)
            {
                return;
            }

            player.MovementContext.IsGrounded = true;

            var vect = new Vector3(0.0f, 0.0f, 0.0f);
            player.MovementContext.ApplyGravity(ref vect, 0f, false);

            if (!player.isActiveAndEnabled || player.IsInventoryOpened)
                return;

            var camera = player.GetComponent<PlayerCameraController>().Camera.transform;
            var dir = new Vector3();

            // Horizontal
            if (UnityInput.Current.GetKey(KeyCode.W))
                dir += camera.forward;
            if (UnityInput.Current.GetKey(KeyCode.S))
                dir += -camera.forward;
            if (UnityInput.Current.GetKey(KeyCode.D))
                dir += camera.right;
            if (UnityInput.Current.GetKey(KeyCode.A))
                dir += -camera.right;

            // Vertical
            if (UnityInput.Current.GetKey(KeyCode.Space))
                dir.y += camera.up.y;
            if (UnityInput.Current.GetKey(KeyCode.LeftControl))
                dir.y += -camera.up.y;

            var prevPos = player.Transform.localPosition;
            if (prevPos.Equals(Vector3.zero))
                return;
            
            var newPos = prevPos + dir * (Plugin.NoClipSpeed.Value * Time.deltaTime);

            //player.Transform.position = newPos;
            player.Transform.localPosition = newPos;
        }
    }
}
