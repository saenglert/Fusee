//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.8
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace C4d {

public class GeSignal : global::System.IDisposable {
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal GeSignal(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(GeSignal obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          throw new global::System.MethodAccessException("C++ destructor does not have public access");
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
    }
  }

  public static GeSignal Alloc() {
    global::System.IntPtr cPtr = C4dApiPINVOKE.GeSignal_Alloc();
    GeSignal ret = (cPtr == global::System.IntPtr.Zero) ? null : new GeSignal(cPtr, false);
    return ret;
  }

  public static void Free(SWIGTYPE_p_p_GeSignal sm) {
    C4dApiPINVOKE.GeSignal_Free(SWIGTYPE_p_p_GeSignal.getCPtr(sm));
    if (C4dApiPINVOKE.SWIGPendingException.Pending) throw C4dApiPINVOKE.SWIGPendingException.Retrieve();
  }

  public bool Init(SIGNALMODE mode) {
    bool ret = C4dApiPINVOKE.GeSignal_Init__SWIG_0(swigCPtr, (int)mode);
    return ret;
  }

  public bool Init() {
    bool ret = C4dApiPINVOKE.GeSignal_Init__SWIG_1(swigCPtr);
    return ret;
  }

  public void Set() {
    C4dApiPINVOKE.GeSignal_Set(swigCPtr);
  }

  public void Clear() {
    C4dApiPINVOKE.GeSignal_Clear(swigCPtr);
  }

  public bool Wait(int timeout) {
    bool ret = C4dApiPINVOKE.GeSignal_Wait(swigCPtr, timeout);
    return ret;
  }

}

}
