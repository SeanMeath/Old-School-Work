package ca.qc.johnabbott.cs616.noteapplication.ui.util;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.DialogFragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import java.util.List;
import ca.qc.johnabbott.cs616.noteapplication.R;
import ca.qc.johnabbott.cs616.noteapplication.model.DatabaseHandler;
import ca.qc.johnabbott.cs616.noteapplication.model.OnCollaboratorAdded;
import ca.qc.johnabbott.cs616.noteapplication.model.User;

/**
 * A dialog showing a list of users from the database
 */
public class AddCollaboratorDialogFragment extends DialogFragment {

    // UI component references
    private RecyclerView collaboratorsRecyclerView;
    private Button doneButton;

    // data source and data adapter
    private List<User> collaborators;
    private CollaboratorAdapter adapter;

    //Allow for callback to edit note
    private OnCollaboratorAdded noteActivityFragment;

    public AddCollaboratorDialogFragment() {
    }

    public AddCollaboratorDialogFragment(List<User> collaborators, OnCollaboratorAdded noteActivityFragment) {
        this.collaborators = collaborators;
        this.noteActivityFragment = noteActivityFragment;
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View root = inflater.inflate(R.layout.fragment_dialog_add_collaborator, container, false);

        collaboratorsRecyclerView = root.findViewById(R.id.collaborators_RecyclerView);
        adapter = new CollaboratorAdapter();
        collaboratorsRecyclerView.setAdapter(adapter);
        collaboratorsRecyclerView.setLayoutManager(new LinearLayoutManager(getContext()));
        collaboratorsRecyclerView.setHasFixedSize(true);

        doneButton = root.findViewById(R.id.done_Button);
        doneButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                AddCollaboratorDialogFragment.this.dismiss();
            }
        });
        return root;
    }

    /**
     * Curtom adapter to display users
     */
    private class CollaboratorAdapter extends RecyclerView.Adapter<AddCollaboratorDialogFragment.CollaboratorViewHolder> {
        @NonNull
        @Override
        public AddCollaboratorDialogFragment.CollaboratorViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
            return new AddCollaboratorDialogFragment.CollaboratorViewHolder(
                    LayoutInflater.from(parent.getContext())
                            .inflate(R.layout.list_item_add_collaborator, parent, false)
            );
        }

        @Override
        public void onBindViewHolder(@NonNull AddCollaboratorDialogFragment.CollaboratorViewHolder holder, int position) {
            holder.set(collaborators.get(position));
        }

        @Override
        public int getItemCount() {
            return collaborators.size();
        }
    }

    /**
     * Cursomt view holder to display a clickable user
     */
    private class CollaboratorViewHolder extends RecyclerView.ViewHolder {

        private final TextView collaboratorNameTextView;
        private final ImageView avatarImageView;
        private User user;

        public CollaboratorViewHolder(@NonNull View itemView) {
            super(itemView);
            collaboratorNameTextView = itemView.findViewById(R.id.collaboratorName_TextView);
            avatarImageView = itemView.findViewById(R.id.avatar_ImageView);
            itemView.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    // remove the clicked item from the list
                   try {
                       //Get the position of the collaborator to add / remove
                       int position = collaborators.indexOf(user);

                       //Initiate Callback
                        noteActivityFragment.onCollaboratorAdded(collaborators.get(position));

                       collaborators.remove(position);
                       adapter.notifyItemRemoved(position);
                   }
                   catch (Exception e){
                   }
                   }
            });
        }

        /**
         * Set the user of the view holder.
         * @param user
         */
        public void set(User user) {
            this.user = user;
            collaboratorNameTextView.setText(user.getName());
            avatarImageView.setImageBitmap(user.getAvatar());
        }
    }
}
