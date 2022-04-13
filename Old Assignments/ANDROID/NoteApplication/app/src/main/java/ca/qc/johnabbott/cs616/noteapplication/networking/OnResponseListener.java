package ca.qc.johnabbott.cs616.noteapplication.networking;

public interface OnResponseListener<T> {
    void onResponse(T data);
}
