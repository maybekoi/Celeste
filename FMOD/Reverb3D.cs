// Decompiled with JetBrains decompiler
// Type: FMOD.Reverb3D
// Assembly: Celeste, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FAF6CA25-5C06-43EB-A08F-9CCF291FE6A3
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Celeste\Celeste.exe

using System;
using System.Runtime.InteropServices;

namespace FMOD
{
    public class Reverb3D : HandleBase
    {
        public RESULT release()
        {
            int num = (int)Reverb3D.FMOD_Reverb3D_Release(this.getRaw());
            if (num != 0)
                return (RESULT)num;
            this.rawPtr = IntPtr.Zero;
            return (RESULT)num;
        }

        public RESULT set3DAttributes(ref VECTOR position, float mindistance, float maxdistance) => Reverb3D.FMOD_Reverb3D_Set3DAttributes(this.rawPtr, ref position, mindistance, maxdistance);

        public RESULT get3DAttributes(
            ref VECTOR position,
            ref float mindistance,
            ref float maxdistance)
        {
            return Reverb3D.FMOD_Reverb3D_Get3DAttributes(this.rawPtr, ref position, ref mindistance, ref maxdistance);
        }

        public RESULT setProperties(ref REVERB_PROPERTIES properties) => Reverb3D.FMOD_Reverb3D_SetProperties(this.rawPtr, ref properties);

        public RESULT getProperties(ref REVERB_PROPERTIES properties) => Reverb3D.FMOD_Reverb3D_GetProperties(this.rawPtr, ref properties);

        public RESULT setActive(bool active) => Reverb3D.FMOD_Reverb3D_SetActive(this.rawPtr, active);

        public RESULT getActive(out bool active) => Reverb3D.FMOD_Reverb3D_GetActive(this.rawPtr, out active);

        public RESULT setUserData(IntPtr userdata) => Reverb3D.FMOD_Reverb3D_SetUserData(this.rawPtr, userdata);

        public RESULT getUserData(out IntPtr userdata) => Reverb3D.FMOD_Reverb3D_GetUserData(this.rawPtr, out userdata);

        [DllImport("fmod")]
        private static extern RESULT FMOD_Reverb3D_Release(IntPtr reverb);

        [DllImport("fmod")]
        private static extern RESULT FMOD_Reverb3D_Set3DAttributes(
            IntPtr reverb,
            ref VECTOR position,
            float mindistance,
            float maxdistance);

        [DllImport("fmod")]
        private static extern RESULT FMOD_Reverb3D_Get3DAttributes(
            IntPtr reverb,
            ref VECTOR position,
            ref float mindistance,
            ref float maxdistance);

        [DllImport("fmod")]
        private static extern RESULT FMOD_Reverb3D_SetProperties(
            IntPtr reverb,
            ref REVERB_PROPERTIES properties);

        [DllImport("fmod")]
        private static extern RESULT FMOD_Reverb3D_GetProperties(
            IntPtr reverb,
            ref REVERB_PROPERTIES properties);

        [DllImport("fmod")]
        private static extern RESULT FMOD_Reverb3D_SetActive(IntPtr reverb, bool active);

        [DllImport("fmod")]
        private static extern RESULT FMOD_Reverb3D_GetActive(IntPtr reverb, out bool active);

        [DllImport("fmod")]
        private static extern RESULT FMOD_Reverb3D_SetUserData(IntPtr reverb, IntPtr userdata);

        [DllImport("fmod")]
        private static extern RESULT FMOD_Reverb3D_GetUserData(IntPtr reverb, out IntPtr userdata);

        public Reverb3D(IntPtr raw)
            : base(raw)
        {
        }
    }
}