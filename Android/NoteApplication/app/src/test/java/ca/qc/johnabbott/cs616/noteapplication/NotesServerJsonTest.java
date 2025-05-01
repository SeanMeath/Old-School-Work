package ca.qc.johnabbott.cs616.noteapplication;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.util.Base64;
import org.junit.Before;
import org.junit.Test;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import ca.qc.johnabbott.cs616.noteapplication.model.Category;
import ca.qc.johnabbott.cs616.noteapplication.model.Collaborator;
import ca.qc.johnabbott.cs616.noteapplication.model.Note;
import ca.qc.johnabbott.cs616.noteapplication.model.User;
import static junit.framework.TestCase.assertEquals;

public class NotesServerJsonTest {

    private Note note;
    private SimpleDateFormat dateFormat;

    @Before
    public void setUp() throws ParseException {
        dateFormat = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss.SSSZ");
    }

    /*
    Check that two notes are equal _without_ calling .equals, in case that's buggy.
    Also, it helps to know which fields are not the equal.
     */
    private void assertNotesEqual(Note expected, Note actual) {
        assertEquals(expected.getUuid(), actual.getUuid());
        assertEquals(expected.getTitle(), actual.getTitle());
        assertEquals(expected.getBody(), actual.getBody());
        assertEquals(expected.isHasReminder(), actual.isHasReminder());
        assertEquals(expected.getReminder(), actual.getReminder());
        assertEquals(expected.getCreated(), actual.getCreated());
        assertEquals(expected.getModified(), actual.getModified());
        assertEquals(expected.getCategory(), actual.getCategory());
    }

    private void assertUsersEqual(User expected, User actual) {
        assertEquals(expected.getUuid(), actual.getUuid());
        assertEquals(expected.getName(), actual.getName());
        assertEquals(expected.getAvatar(), actual.getAvatar());
        assertEquals(expected.getEmail(), actual.getEmail());
    }

    @Test
    public void testPostJson() throws ParseException {
        String expected = "{\"title\":\"Foo\",\"body\":\"foo\",\"category\":\"RED\",\"reminder\":\"2018-11-06T07:32:12.123-0500\",\"created\":\"2018-11-04T12:34:45.000-0500\",\"modified\":\"2018-11-05T13:42:44.000-0500\"}";
        String actual = new Note()
                .setUuid("e187ecea-167c-4feb-a13a-eb34949bb400")
                .setCreated(dateFormat.parse("2018-11-04T17:34:45.000+0000"))
                .setHasReminder(true)
                .setReminder(dateFormat.parse("2018-11-06T12:32:12.123+0000"))
                .setModified(dateFormat.parse("2018-11-05T18:42:44.000+0000"))
                .setTitle("Foo")
                .setBody("foo")
                .setCategory(Category.RED)
                .format();
        assertEquals(expected, actual);
    }

    @Test
    public void testNoteGetJson() throws ParseException {
        String json = "{\n" +
                "  \"created\" : \"2018-11-04T17:34:45.000+0000\",\n" +
                "  \"reminder\" : \"2018-11-06T12:32:12.123+0000\",\n" +
                "  \"modified\" : \"2018-11-05T18:42:44.000+0000\",\n" +
                "  \"title\" : \"Foo\",\n" +
                "  \"body\" : \"foo\",\n" +
                "  \"category\" : \"RED\",\n" +
                "  \"_embedded\" : {\n" +
                "    \"collaborators\" : [ {\n" +
                "      \"name\" : \"Ian Clement\",\n" +
                "      \"uuid\" : \"7bdba0fe-fe95-4b1c-8247-f2479ee6e380\",\n" +
                "      \"email\" : \"ian@foo.com\",\n" +
                "      \"_links\" : {\n" +
                "        \"self\" : {\n" +
                "          \"href\" : \"http://localhost:9999/user/7bdba0fe-fe95-4b1c-8247-f2479ee6e380{?projection}\",\n" +
                "          \"templated\" : true\n" +
                "        },\n" +
                "        \"notes\" : {\n" +
                "          \"href\" : \"http://localhost:9999/user/7bdba0fe-fe95-4b1c-8247-f2479ee6e380/notes\"\n" +
                "        }\n" +
                "      }\n" +
                "    }, {\n" +
                "      \"name\" : \"Aref Mourtada\",\n" +
                "      \"uuid\" : \"6e840afc-5c0a-4679-bcfa-8a210e50ecfc\",\n" +
                "      \"email\" : \"aref@foo.com\",\n" +
                "      \"_links\" : {\n" +
                "        \"self\" : {\n" +
                "          \"href\" : \"http://localhost:9999/user/6e840afc-5c0a-4679-bcfa-8a210e50ecfc{?projection}\",\n" +
                "          \"templated\" : true\n" +
                "        },\n" +
                "        \"notes\" : {\n" +
                "          \"href\" : \"http://localhost:9999/user/6e840afc-5c0a-4679-bcfa-8a210e50ecfc/notes\"\n" +
                "        }\n" +
                "      }\n" +
                "    }, {\n" +
                "      \"name\" : \"Sandy Bultena\",\n" +
                "      \"uuid\" : \"2c77dafe-1545-432f-b5b1-3a0011cf7036\",\n" +
                "      \"email\" : \"sandy@foo.com\",\n" +
                "      \"_links\" : {\n" +
                "        \"self\" : {\n" +
                "          \"href\" : \"http://localhost:9999/user/2c77dafe-1545-432f-b5b1-3a0011cf7036{?projection}\",\n" +
                "          \"templated\" : true\n" +
                "        },\n" +
                "        \"notes\" : {\n" +
                "          \"href\" : \"http://localhost:9999/user/2c77dafe-1545-432f-b5b1-3a0011cf7036/notes\"\n" +
                "        }\n" +
                "      }\n" +
                "    } ]\n" +
                "  },\n" +
                "  \"_links\" : {\n" +
                "    \"self\" : {\n" +
                "      \"href\" : \"http://localhost:9999/note/e187ecea-167c-4feb-a13a-eb34949bb400\"\n" +
                "    },\n" +
                "    \"note\" : {\n" +
                "      \"href\" : \"http://localhost:9999/note/e187ecea-167c-4feb-a13a-eb34949bb400\"\n" +
                "    },\n" +
                "    \"collaborators\" : {\n" +
                "      \"href\" : \"http://localhost:9999/note/e187ecea-167c-4feb-a13a-eb34949bb400/collaborators{?projection}\",\n" +
                "      \"templated\" : true\n" +
                "    }\n" +
                "  }\n" +
                "}";

        Note expected = new Note()
                .setUuid("e187ecea-167c-4feb-a13a-eb34949bb400")
                .setCreated(dateFormat.parse("2018-11-04T17:34:45.000+0000"))
                .setHasReminder(true)
                .setReminder(dateFormat.parse("2018-11-06T12:32:12.123+0000"))
                .setModified(dateFormat.parse("2018-11-05T18:42:44.000+0000"))
                .setTitle("Foo")
                .setBody("foo")
                .setCategory(Category.RED);

        Note actual = Note.parse(json);

        assertNotesEqual(expected, actual);
    }

    @Test
    public void testNoteGetJsonNoReminder() throws ParseException {

        String json = "{\n" +
                "  \"created\" : \"2018-11-04T17:34:45.000+0000\",\n" +
                "  \"reminder\" : null,\n" +
                "  \"modified\" : \"2018-11-05T18:42:44.000+0000\",\n" +
                "  \"title\" : \"Foo\",\n" +
                "  \"body\" : \"foo\",\n" +
                "  \"category\" : \"RED\",\n" +
                "  \"_embedded\" : {\n" +
                "    \"collaborators\" : [ {\n" +
                "      \"name\" : \"Ian Clement\",\n" +
                "      \"uuid\" : \"7bdba0fe-fe95-4b1c-8247-f2479ee6e380\",\n" +
                "      \"email\" : \"ian@foo.com\",\n" +
                "      \"_links\" : {\n" +
                "        \"self\" : {\n" +
                "          \"href\" : \"http://localhost:9999/user/7bdba0fe-fe95-4b1c-8247-f2479ee6e380{?projection}\",\n" +
                "          \"templated\" : true\n" +
                "        },\n" +
                "        \"notes\" : {\n" +
                "          \"href\" : \"http://localhost:9999/user/7bdba0fe-fe95-4b1c-8247-f2479ee6e380/notes\"\n" +
                "        }\n" +
                "      }\n" +
                "    }, {\n" +
                "      \"name\" : \"Aref Mourtada\",\n" +
                "      \"uuid\" : \"6e840afc-5c0a-4679-bcfa-8a210e50ecfc\",\n" +
                "      \"email\" : \"aref@foo.com\",\n" +
                "      \"_links\" : {\n" +
                "        \"self\" : {\n" +
                "          \"href\" : \"http://localhost:9999/user/6e840afc-5c0a-4679-bcfa-8a210e50ecfc{?projection}\",\n" +
                "          \"templated\" : true\n" +
                "        },\n" +
                "        \"notes\" : {\n" +
                "          \"href\" : \"http://localhost:9999/user/6e840afc-5c0a-4679-bcfa-8a210e50ecfc/notes\"\n" +
                "        }\n" +
                "      }\n" +
                "    }, {\n" +
                "      \"name\" : \"Sandy Bultena\",\n" +
                "      \"uuid\" : \"2c77dafe-1545-432f-b5b1-3a0011cf7036\",\n" +
                "      \"email\" : \"sandy@foo.com\",\n" +
                "      \"_links\" : {\n" +
                "        \"self\" : {\n" +
                "          \"href\" : \"http://localhost:9999/user/2c77dafe-1545-432f-b5b1-3a0011cf7036{?projection}\",\n" +
                "          \"templated\" : true\n" +
                "        },\n" +
                "        \"notes\" : {\n" +
                "          \"href\" : \"http://localhost:9999/user/2c77dafe-1545-432f-b5b1-3a0011cf7036/notes\"\n" +
                "        }\n" +
                "      }\n" +
                "    } ]\n" +
                "  },\n" +
                "  \"_links\" : {\n" +
                "    \"self\" : {\n" +
                "      \"href\" : \"http://localhost:9999/note/e187ecea-167c-4feb-a13a-eb34949bb400\"\n" +
                "    },\n" +
                "    \"note\" : {\n" +
                "      \"href\" : \"http://localhost:9999/note/e187ecea-167c-4feb-a13a-eb34949bb400\"\n" +
                "    },\n" +
                "    \"collaborators\" : {\n" +
                "      \"href\" : \"http://localhost:9999/note/e187ecea-167c-4feb-a13a-eb34949bb400/collaborators{?projection}\",\n" +
                "      \"templated\" : true\n" +
                "    }\n" +
                "  }\n" +
                "}";

        Note expected = new Note()
                .setUuid("e187ecea-167c-4feb-a13a-eb34949bb400")
                .setCreated(dateFormat.parse("2018-11-04T17:34:45.000+0000"))
                .setHasReminder(false)
                .setModified(dateFormat.parse("2018-11-05T18:42:44.000+0000"))
                .setTitle("Foo")
                .setBody("foo")
                .setCategory(Category.RED);

        Note actual = Note.parse(json);

        assertNotesEqual(expected, actual);
    }

    @Test
    public void testNoteArrayGetJson() throws ParseException {
        String json = "{\n" +
                "  \"_embedded\": {\n" +
                "    \"notes\": [\n" +
                "      {\n" +
                "        \"created\": \"2018-11-04T17:34:45.000+0000\",\n" +
                "        \"reminder\": null,\n" +
                "        \"modified\": \"2018-11-05T18:42:44.000+0000\",\n" +
                "        \"title\": \"Foo\",\n" +
                "        \"body\": \"foo\",\n" +
                "        \"category\": \"RED\",\n" +
                "        \"_embedded\": {\n" +
                "          \"collaborators\": [\n" +
                "            {\n" +
                "              \"name\": \"Ian Clement\",\n" +
                "              \"uuid\": \"7bdba0fe-fe95-4b1c-8247-f2479ee6e380\",\n" +
                "              \"email\": \"ian@foo.com\",\n" +
                "              \"_links\": {\n" +
                "                \"self\": {\n" +
                "                  \"href\": \"http://localhost:9999/user/7bdba0fe-fe95-4b1c-8247-f2479ee6e380{?projection}\",\n" +
                "                  \"templated\": true\n" +
                "                },\n" +
                "                \"notes\": {\n" +
                "                  \"href\": \"http://localhost:9999/user/7bdba0fe-fe95-4b1c-8247-f2479ee6e380/notes\"\n" +
                "                }\n" +
                "              }\n" +
                "            },\n" +
                "            {\n" +
                "              \"name\": \"Aref Mourtada\",\n" +
                "              \"uuid\": \"6e840afc-5c0a-4679-bcfa-8a210e50ecfc\",\n" +
                "              \"email\": \"aref@foo.com\",\n" +
                "              \"_links\": {\n" +
                "                \"self\": {\n" +
                "                  \"href\": \"http://localhost:9999/user/6e840afc-5c0a-4679-bcfa-8a210e50ecfc{?projection}\",\n" +
                "                  \"templated\": true\n" +
                "                },\n" +
                "                \"notes\": {\n" +
                "                  \"href\": \"http://localhost:9999/user/6e840afc-5c0a-4679-bcfa-8a210e50ecfc/notes\"\n" +
                "                }\n" +
                "              }\n" +
                "            },\n" +
                "            {\n" +
                "              \"name\": \"Sandy Bultena\",\n" +
                "              \"uuid\": \"2c77dafe-1545-432f-b5b1-3a0011cf7036\",\n" +
                "              \"email\": \"sandy@foo.com\",\n" +
                "              \"_links\": {\n" +
                "                \"self\": {\n" +
                "                  \"href\": \"http://localhost:9999/user/2c77dafe-1545-432f-b5b1-3a0011cf7036{?projection}\",\n" +
                "                  \"templated\": true\n" +
                "                },\n" +
                "                \"notes\": {\n" +
                "                  \"href\": \"http://localhost:9999/user/2c77dafe-1545-432f-b5b1-3a0011cf7036/notes\"\n" +
                "                }\n" +
                "              }\n" +
                "            }\n" +
                "          ]\n" +
                "        },\n" +
                "        \"_links\": {\n" +
                "          \"self\": {\n" +
                "            \"href\": \"http://localhost:9999/note/e187ecea-167c-4feb-a13a-eb34949bb400\"\n" +
                "          },\n" +
                "          \"note\": {\n" +
                "            \"href\": \"http://localhost:9999/note/e187ecea-167c-4feb-a13a-eb34949bb400\"\n" +
                "          },\n" +
                "          \"collaborators\": {\n" +
                "            \"href\": \"http://localhost:9999/note/e187ecea-167c-4feb-a13a-eb34949bb400/collaborators{?projection}\",\n" +
                "            \"templated\": true\n" +
                "          }\n" +
                "        }\n" +
                "      },\n" +
                "      {\n" +
                "        \"created\": \"2018-11-04T17:34:45.000+0000\",\n" +
                "        \"reminder\": \"2018-11-23T14:12:12.000+0000\",\n" +
                "        \"modified\": \"2018-11-05T18:42:44.000+0000\",\n" +
                "        \"title\": \"Bar\",\n" +
                "        \"body\": \"bar\",\n" +
                "        \"category\": \"GREEN\",\n" +
                "        \"_embedded\": {\n" +
                "          \"collaborators\": [\n" +
                "            {\n" +
                "              \"name\": \"Ian Clement\",\n" +
                "              \"uuid\": \"7bdba0fe-fe95-4b1c-8247-f2479ee6e380\",\n" +
                "              \"email\": \"ian@foo.com\",\n" +
                "              \"_links\": {\n" +
                "                \"self\": {\n" +
                "                  \"href\": \"http://localhost:9999/user/7bdba0fe-fe95-4b1c-8247-f2479ee6e380{?projection}\",\n" +
                "                  \"templated\": true\n" +
                "                },\n" +
                "                \"notes\": {\n" +
                "                  \"href\": \"http://localhost:9999/user/7bdba0fe-fe95-4b1c-8247-f2479ee6e380/notes\"\n" +
                "                }\n" +
                "              }\n" +
                "            },\n" +
                "            {\n" +
                "              \"name\": \"Aref Mourtada\",\n" +
                "              \"uuid\": \"6e840afc-5c0a-4679-bcfa-8a210e50ecfc\",\n" +
                "              \"email\": \"aref@foo.com\",\n" +
                "              \"_links\": {\n" +
                "                \"self\": {\n" +
                "                  \"href\": \"http://localhost:9999/user/6e840afc-5c0a-4679-bcfa-8a210e50ecfc{?projection}\",\n" +
                "                  \"templated\": true\n" +
                "                },\n" +
                "                \"notes\": {\n" +
                "                  \"href\": \"http://localhost:9999/user/6e840afc-5c0a-4679-bcfa-8a210e50ecfc/notes\"\n" +
                "                }\n" +
                "              }\n" +
                "            }\n" +
                "          ]\n" +
                "        },\n" +
                "        \"_links\": {\n" +
                "          \"self\": {\n" +
                "            \"href\": \"http://localhost:9999/note/0615d64b-eab3-4ac7-bcfb-6b2b015800d8\"\n" +
                "          },\n" +
                "          \"note\": {\n" +
                "            \"href\": \"http://localhost:9999/note/0615d64b-eab3-4ac7-bcfb-6b2b015800d8\"\n" +
                "          },\n" +
                "          \"collaborators\": {\n" +
                "            \"href\": \"http://localhost:9999/note/0615d64b-eab3-4ac7-bcfb-6b2b015800d8/collaborators{?projection}\",\n" +
                "            \"templated\": true\n" +
                "          }\n" +
                "        }\n" +
                "      },\n" +
                "      {\n" +
                "        \"created\": \"2018-11-15T20:30:45.000+0000\",\n" +
                "        \"reminder\": null,\n" +
                "        \"modified\": \"2018-11-18T18:00:40.000+0000\",\n" +
                "        \"title\": \"Quux\",\n" +
                "        \"body\": \"quux\",\n" +
                "        \"category\": \"ORANGE\",\n" +
                "        \"_embedded\": {\n" +
                "          \"collaborators\": [\n" +
                "            {\n" +
                "              \"name\": \"Aref Mourtada\",\n" +
                "              \"uuid\": \"6e840afc-5c0a-4679-bcfa-8a210e50ecfc\",\n" +
                "              \"email\": \"aref@foo.com\",\n" +
                "              \"_links\": {\n" +
                "                \"self\": {\n" +
                "                  \"href\": \"http://localhost:9999/user/6e840afc-5c0a-4679-bcfa-8a210e50ecfc{?projection}\",\n" +
                "                  \"templated\": true\n" +
                "                },\n" +
                "                \"notes\": {\n" +
                "                  \"href\": \"http://localhost:9999/user/6e840afc-5c0a-4679-bcfa-8a210e50ecfc/notes\"\n" +
                "                }\n" +
                "              }\n" +
                "            }\n" +
                "          ]\n" +
                "        },\n" +
                "        \"_links\": {\n" +
                "          \"self\": {\n" +
                "            \"href\": \"http://localhost:9999/note/cb6e6a4b-3146-456b-a2cc-27c9cb39401c\"\n" +
                "          },\n" +
                "          \"note\": {\n" +
                "            \"href\": \"http://localhost:9999/note/cb6e6a4b-3146-456b-a2cc-27c9cb39401c\"\n" +
                "          },\n" +
                "          \"collaborators\": {\n" +
                "            \"href\": \"http://localhost:9999/note/cb6e6a4b-3146-456b-a2cc-27c9cb39401c/collaborators{?projection}\",\n" +
                "            \"templated\": true\n" +
                "          }\n" +
                "        }\n" +
                "      },\n" +
                "      {\n" +
                "        \"created\": \"2018-11-05T13:10:45.000+0000\",\n" +
                "        \"reminder\": \"2018-11-14T17:12:14.000+0000\",\n" +
                "        \"modified\": \"2018-11-05T18:42:44.000+0000\",\n" +
                "        \"title\": \"Garply\",\n" +
                "        \"body\": \"garply\",\n" +
                "        \"category\": \"LIGHT_BLUE\",\n" +
                "        \"_links\": {\n" +
                "          \"self\": {\n" +
                "            \"href\": \"http://localhost:9999/note/56cc7c1a-8cd4-48db-b690-7ac592a5fc02\"\n" +
                "          },\n" +
                "          \"note\": {\n" +
                "            \"href\": \"http://localhost:9999/note/56cc7c1a-8cd4-48db-b690-7ac592a5fc02\"\n" +
                "          },\n" +
                "          \"collaborators\": {\n" +
                "            \"href\": \"http://localhost:9999/note/56cc7c1a-8cd4-48db-b690-7ac592a5fc02/collaborators{?projection}\",\n" +
                "            \"templated\": true\n" +
                "          }\n" +
                "        }\n" +
                "      }\n" +
                "    ]\n" +
                "  },\n" +
                "  \"_links\": {\n" +
                "    \"self\": {\n" +
                "      \"href\": \"http://localhost:9999/note\"\n" +
                "    },\n" +
                "    \"profile\": {\n" +
                "      \"href\": \"http://localhost:9999/profile/note\"\n" +
                "    }\n" +
                "  }\n" +
                "}\n" +
                "\n";

        Note expected1 = new Note()
                .setUuid("e187ecea-167c-4feb-a13a-eb34949bb400")
                .setTitle("Foo")
                .setBody("foo")
                .setCategory(Category.RED)
                .setReminder(null)
                .setHasReminder(false)
                .setCreated(dateFormat.parse("2018-11-04T17:34:45.000+0000"))
                .setModified(dateFormat.parse("2018-11-05T18:42:44.000+0000"));

        Note expected2 = new Note()
                .setUuid("0615d64b-eab3-4ac7-bcfb-6b2b015800d8")
                .setTitle("Bar")
                .setBody("bar")
                .setCategory(Category.GREEN)
                .setReminder(dateFormat.parse("2018-11-23T14:12:12.000+0000"))
                .setHasReminder(true)
                .setCreated(dateFormat.parse("2018-11-04T17:34:45.000+0000"))
                .setModified(dateFormat.parse("2018-11-05T18:42:44.000+0000"));

        Note[] actual = Note.parseArray(json);
        assertEquals(4, actual.length);
        assertNotesEqual(expected1, actual[0]);
        assertNotesEqual(expected2, actual[1]);
    }

    @Test
    public void testUserGetJson() throws ParseException {
        String json = "{\n" +
                "  \"email\": \"ian@foo.com\",\n" +
                "  \"name\": \"Ian Clement\",\n" +
                "  \"avatar\": \"iVBORw0KGgoAAAANSUhEUgAABBoAAAQaCAYAAADt3BtfAAABEWlDQ1B\",\n" + // truncated for test
                "  \"password\": null,\n" +
                "  \"_embedded\": {\n" +
                "    \"notes\": [\n" +
                "      {\n" +
                "        \"created\": \"2018-11-04T17:34:45.000+0000\",\n" +
                "        \"reminder\": null,\n" +
                "        \"modified\": \"2018-11-05T18:42:44.000+0000\",\n" +
                "        \"title\": \"Server Note 1\",\n" +
                "        \"body\": \"Body 1\",\n" +
                "        \"category\": \"RED\"\n" +
                "      },\n" +
                "      {\n" +
                "        \"created\": \"2018-11-04T17:34:45.000+0000\",\n" +
                "        \"reminder\": \"2018-11-23T14:12:12.000+0000\",\n" +
                "        \"modified\": \"2018-11-05T18:42:44.000+0000\",\n" +
                "        \"title\": \"Server Note 2\",\n" +
                "        \"body\": \"Body 2\",\n" +
                "        \"category\": \"GREEN\"\n" +
                "      }\n" +
                "    ]\n" +
                "  },\n" +
                "  \"_links\": {\n" +
                "    \"self\": {\n" +
                "      \"href\": \"http://localhost:9999/user/7bdba0fe-fe95-4b1c-8247-f2479ee6e380\"\n" +
                "    },\n" +
                "    \"user\": {\n" +
                "      \"href\": \"http://localhost:9999/user/7bdba0fe-fe95-4b1c-8247-f2479ee6e380{?projection}\",\n" +
                "      \"templated\": true\n" +
                "    },\n" +
                "    \"notes\": {\n" +
                "      \"href\": \"http://localhost:9999/user/7bdba0fe-fe95-4b1c-8247-f2479ee6e380/notes\"\n" +
                "    }\n" +
                "  }\n" +
                "}\n";
        //CRASH HERE
        byte[] bytes = Base64.decode("iVBORw0KGgoAAAANSUhEUgAABBoAAAQaCAYAAADt3BtfAAABEWlDQ1B", Base64.NO_WRAP);
        Bitmap bitmap = BitmapFactory.decodeByteArray(bytes, 0, bytes.length);

        User expected = new User()
                .setUuid("7bdba0fe-fe95-4b1c-8247-f2479ee6e380")
                .setName("Ian Clement")
                .setEmail("ian@foo.com")
                .setAvatar(bitmap);

        User actual = User.parse(json);

        assertUsersEqual(expected, actual);
    }

    @Test
    public void testUserArrayGetJson() {
        String json = "{\n" +
                "  \"_embedded\": {\n" +
                "    \"users\": [\n" +
                "      {\n" +
                "        \"name\": \"Ian Clement\",\n" +
                "        \"uuid\": \"7bdba0fe-fe95-4b1c-8247-f2479ee6e380\",\n" +
                "        \"email\": \"ian@foo.com\",\n" +
                "        \"_links\": {\n" +
                "          \"self\": {\n" +
                "            \"href\": \"http://localhost:9999/user/7bdba0fe-fe95-4b1c-8247-f2479ee6e380\"\n" +
                "          },\n" +
                "          \"user\": {\n" +
                "            \"href\": \"http://localhost:9999/user/7bdba0fe-fe95-4b1c-8247-f2479ee6e380{?projection}\",\n" +
                "            \"templated\": true\n" +
                "          },\n" +
                "          \"notes\": {\n" +
                "            \"href\": \"http://localhost:9999/user/7bdba0fe-fe95-4b1c-8247-f2479ee6e380/notes\"\n" +
                "          }\n" +
                "        }\n" +
                "      },\n" +
                "      {\n" +
                "        \"name\": \"Aref Mourtada\",\n" +
                "        \"uuid\": \"6e840afc-5c0a-4679-bcfa-8a210e50ecfc\",\n" +
                "        \"email\": \"aref@foo.com\",\n" +
                "        \"_links\": {\n" +
                "          \"self\": {\n" +
                "            \"href\": \"http://localhost:9999/user/6e840afc-5c0a-4679-bcfa-8a210e50ecfc\"\n" +
                "          },\n" +
                "          \"user\": {\n" +
                "            \"href\": \"http://localhost:9999/user/6e840afc-5c0a-4679-bcfa-8a210e50ecfc{?projection}\",\n" +
                "            \"templated\": true\n" +
                "          },\n" +
                "          \"notes\": {\n" +
                "            \"href\": \"http://localhost:9999/user/6e840afc-5c0a-4679-bcfa-8a210e50ecfc/notes\"\n" +
                "          }\n" +
                "        }\n" +
                "      },\n" +
                "      {\n" +
                "        \"name\": \"Sandy Bultena\",\n" +
                "        \"uuid\": \"2c77dafe-1545-432f-b5b1-3a0011cf7036\",\n" +
                "        \"email\": \"sandy@foo.com\",\n" +
                "        \"_links\": {\n" +
                "          \"self\": {\n" +
                "            \"href\": \"http://localhost:9999/user/2c77dafe-1545-432f-b5b1-3a0011cf7036\"\n" +
                "          },\n" +
                "          \"user\": {\n" +
                "            \"href\": \"http://localhost:9999/user/2c77dafe-1545-432f-b5b1-3a0011cf7036{?projection}\",\n" +
                "            \"templated\": true\n" +
                "          },\n" +
                "          \"notes\": {\n" +
                "            \"href\": \"http://localhost:9999/user/2c77dafe-1545-432f-b5b1-3a0011cf7036/notes\"\n" +
                "          }\n" +
                "        }\n" +
                "      }\n" +
                "    ]\n" +
                "  },\n" +
                "  \"_links\": {\n" +
                "    \"self\": {\n" +
                "      \"href\": \"http://localhost:9999/note/e187ecea-167c-4feb-a13a-eb34949bb400/collaborators\"\n" +
                "    }\n" +
                "  }\n" +
                "}";

        User expected1 = new User()
                .setUuid("7bdba0fe-fe95-4b1c-8247-f2479ee6e380")
                .setName("Ian Clement")
                .setEmail("ian@foo.com")
                .setAvatar(null);

        User expected2 = new User()
                .setUuid("6e840afc-5c0a-4679-bcfa-8a210e50ecfc")
                .setName("Aref Mourtada")
                .setEmail("aref@foo.com")
                .setAvatar(null);

        User[] actual = User.parseArray(json);

        assertEquals(3, actual.length);
        assertUsersEqual(expected1, actual[0]);
        assertUsersEqual(expected2, actual[1]);
    }

    @Test
    public void testCollaboratorPostJson() {
        String expected = "{\"id\":0,\"note\":\"http://localhost:9999/note/cb6e6a4b-3146-456b-a2cc-27c9cb39401c\",\"user\":\"http://localhost:9999/user/2c77dafe-1545-432f-b5b1-3a0011cf7036\"}";
        Collaborator actual = new Collaborator()
                .setNote("http://localhost:9999/note/cb6e6a4b-3146-456b-a2cc-27c9cb39401c")
                .setUser("http://localhost:9999/user/2c77dafe-1545-432f-b5b1-3a0011cf7036");
        assertEquals(expected, actual.format());
    }

}
