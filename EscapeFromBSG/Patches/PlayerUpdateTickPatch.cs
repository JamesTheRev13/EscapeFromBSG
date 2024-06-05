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
    // TODO: Maybe figure out a way to add a timer after disabling NoClip to prevent fall damage insta-kills after disabling NoClip
    public class PlayerUpdateTickPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(Player).GetMethod(nameof(Player.UpdateTick));

        [PatchPrefix]
        private static void Prefix(Player __instance)
        {
            var player = __instance;

            if (player == null || !player.IsYourPlayer)
            {
                return;
            }

            if (Plugin.NoClipShortcut.Value.IsDown())
            {
                Plugin.NoClip.Value = !Plugin.NoClip.Value;
            }

            // NoClip ON -> Disable player fall damage to prevent collision insta-kills if GodMode is NOT enabled with NoClip enabled (NOT RECOMMENDED)
            // NoClip OFF -> Default Fall Damage
            player.ActiveHealthController.FallSafeHeight = Plugin.NoClip.Value ? 9999998f : 2f;

            if (!Plugin.NoClip.Value)
                return;

            //player.CharacterController.attachedRigidbody.detectCollisions = false;
            // Testing collision
            //player.GetTriggerColliderSearcher().IsEnabled = false;
            // OnControllerColliderHit

            //var collider = player?.GetComponent<ICharacterController>() as Collider ?? null;

            //collider.enabled = false;

            //player.MovementContext.ObstacleCollisionFacade
            //player.MovementContext.ObstacleCollisionFacade.Reset();
            //player.HasBodyPartCollider
            //player.Physical.
            //player.UpdateTriggerColliderSearcher

            // TODO: double check if this actually does anyting <.<
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
                dir.y += 1;
            if (UnityInput.Current.GetKey(KeyCode.LeftControl))
                dir.y += -1;
            
            var prevPos = player.Transform.localPosition;
            if (prevPos.Equals(Vector3.zero))
                return;

            var speed = UnityInput.Current.GetKey(KeyCode.LeftShift) ? Plugin.NoClipSpeed.Value * 2f : Plugin.NoClipSpeed.Value;
            
            var newPos = prevPos + dir * (speed * Time.deltaTime);

            player.Transform.localPosition = newPos;
        }
    }
}
