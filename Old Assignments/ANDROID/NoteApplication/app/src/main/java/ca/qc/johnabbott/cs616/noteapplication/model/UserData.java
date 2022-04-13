package ca.qc.johnabbott.cs616.noteapplication.model;
import android.content.Context;
import android.graphics.BitmapFactory;
import java.util.ArrayList;
import java.util.List;
import android.graphics.Bitmap;
import android.os.Build;

import androidx.annotation.RequiresApi;

import ca.qc.johnabbott.cs616.noteapplication.R;

public class UserData {
    private static List<User> data;

    public static List<User> getData(Context current){
        data = new ArrayList<>();

        Bitmap bitmap;
        bitmap = BitmapFactory.decodeResource(current.getResources(), R.drawable.aref);
        data.add(new User(1, "Aref", bitmap, "Aref@johnabbottcollege.net"));
        bitmap = BitmapFactory.decodeResource(current.getResources(), R.drawable.ian);
        data.add(new User(2, "Ian", bitmap, "Ian@johnabbottcollege.net"));
        bitmap = BitmapFactory.decodeResource(current.getResources(), R.drawable.jim);
        data.add(new User(3, "Jim", bitmap, "Jim@johnabbottcollege.net"));
        bitmap = BitmapFactory.decodeResource(current.getResources(), R.drawable.sandy);
        data.add(new User(4, "Sandy", bitmap, "Sandy@johnabbottcollege.net"));
        bitmap = BitmapFactory.decodeResource(current.getResources(), R.drawable.usef);
        data.add(new User(4, "Usef", bitmap, "Usef@johnabbottcollege.net"));

        return data;
    }
}