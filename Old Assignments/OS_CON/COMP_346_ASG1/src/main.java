import Models.Client;
import Models.Network;
import Models.Server;

public class main {

    public static void main(String args[]) {

        System.out.println("test");

        /* Ensure network is initialized and active before starting server */
        Network network = new Network("network");
        network.run();


    }
}
