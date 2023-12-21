using recipesCommon.DataAccess;
using recipesCommon.Interfaces;
using System.Linq;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesAPI.DataAccess
{
    public static class TestDataGenerator
    {
        public static List<string> AuthorNames = new List<string>
        {
            "John Smith",
            "Maria Garcia",
            "Michael Johnson",
            "Laura Martinez",
            "David Brown",
            "Sophia Rodriguez",
            "James Lee",
            "Emma Davis",
            "Robert Kim",
            "Olivia Wilson",
            "William Hernandez",
            "Isabella Lopez",
            "Joseph Nguyen",
            "Mia Walker",
            "Charles Hall",
            "Ava Turner",
            "Thomas Moore",
            "Ella White",
            "Daniel Clark",
            "Grace Lewis",
            "Matthew Baker",
            "Sofia Green",
            "Andrew King",
            "Emily Adams",
            "George Perez",
            "Chloe Lewis",
            "Christopher Allen",
            "Lily Robinson",
            "Nicholas Scott",
            "Zoe King",
            "Josephine Harris",
            "Lucas Wright",
            "Natalie Young",
            "Samuel Hall",
            "Madison Turner",
            "Benjamin Anderson",
            "Avery Mitchell",
            "Alexander Walker",
            "Grace Allen",
            "Henry Young",
            "Scarlett Perez",
            "Edward Baker",
            "Hannah Moore",
            "Victoria Rodriguez",
            "Jack Davis",
            "Elizabeth Hill",
            "Christopher Taylor",
            "Addison Martin",
            "Liam White",
            "Amelia Turner",
            "Anthony Adams",
            "Aria Turner",
            "Dylan Martinez",
            "Evelyn Wright",
            "Nicholas Wright",
            "Sophie Smith",
            "Samuel Lewis",
            "Nora Martinez",
            "David Thomas",
            "Aubrey Perez",
            "Daniel Hall",
            "Lily Clark",
            "William Jackson",
            "Mila Davis",
            "Elijah Lee",
            "Samantha Wilson",
            "Jackson Anderson",
            "Grace Smith",
            "John Wilson",
            "Ella White",
            "James Turner",
            "Hazel Taylor",
            "Ethan Harris",
            "Aria Turner",
            "Benjamin Young",
            "Zoe Allen",
            "Daniel Davis",
            "Chloe Robinson",
            "Christopher Lewis",
            "Mila Walker",
            "Alexander Wilson",
            "Emily Young",
            "Nicholas Smith",
            "Lily Taylor",
            "William Moore",
            "Sophia Robinson",
            "Samuel Davis",
            "Grace White",
            "Joseph Mitchell",
            "Ava Scott",
            "Lucas Green",
            "Scarlett Harris",
            "Henry King",
            "Madison Mitchell",
            "Oliver Martin",
            "Natalie Martinez",
            "Daniel Anderson",
            "Ella Turner",
            "Matthew Hall",
            "Sophie Lewis",
            "David Wright",
            "Sofia Brown",
            "Ethan Turner",
            "Nora Wilson",
            "Michael Adams",
            "Aria Baker",
            "Christopher Smith",
            "Olivia Jackson",
            "William Davis",
            "Grace Martin",
            "Benjamin Perez",
            "Avery Johnson",
            "Alexander Taylor",
            "Ella Walker",
            "Josephine Wright",
            "Lucas Lewis",
            "Liam Smith"
        };

        public static List<string> AddressList = new List<string>
        {
            "www.example.com/image1.jpg",
            "www.example.com/image2.jpg",
            "www.example.com/image3.jpg",
            "www.example.com/image4.jpg",
            "www.example.com/image5.jpg",
            "www.example.com/image6.jpg",
            "www.example.com/image7.jpg",
            "www.example.com/image8.jpg",
            "www.example.com/image9.jpg",
            "www.example.com/image10.jpg",
            "www.example.com/image11.jpg",
            "www.example.com/image12.jpg",
            "www.example.com/image13.jpg",
            "www.example.com/image14.jpg",
            "www.example.com/image15.jpg",
            "www.example.com/image16.jpg",
            "www.example.com/image17.jpg",
            "www.example.com/image18.jpg",
            "www.example.com/image19.jpg",
            "www.example.com/image20.jpg",
            "www.example.com/image21.jpg",
            "www.example.com/image22.jpg",
            "www.example.com/image23.jpg",
            "www.example.com/image24.jpg",
            "www.example.com/image25.jpg",
            "www.example.com/image26.jpg",
            "www.example.com/image27.jpg",
            "www.example.com/image28.jpg",
            "www.example.com/image29.jpg",
            "www.example.com/image30.jpg",
            "www.example.com/image31.jpg",
            "www.example.com/image32.jpg",
            "www.example.com/image33.jpg",
            "www.example.com/image34.jpg",
            "www.example.com/image35.jpg",
            "www.example.com/image36.jpg",
            "www.example.com/image37.jpg",
            "www.example.com/image38.jpg",
            "www.example.com/image39.jpg",
            "www.example.com/image40.jpg",
            "www.example.com/image41.jpg",
            "www.example.com/image42.jpg",
            "www.example.com/image43.jpg",
            "www.example.com/image44.jpg",
            "www.example.com/image45.jpg",
            "www.example.com/image46.jpg",
            "www.example.com/image47.jpg",
            "www.example.com/image48.jpg",
            "www.example.com/image49.jpg",
            "www.example.com/image50.jpg",
            "www.example.com/image51.jpg",
            "www.example.com/image52.jpg",
            "www.example.com/image53.jpg",
            "www.example.com/image54.jpg",
            "www.example.com/image55.jpg",
            "www.example.com/image56.jpg",
            "www.example.com/image57.jpg",
            "www.example.com/image58.jpg",
            "www.example.com/image59.jpg",
            "www.example.com/image60.jpg",
            "www.example.com/image61.jpg",
            "www.example.com/image62.jpg",
            "www.example.com/image63.jpg",
            "www.example.com/image64.jpg",
            "www.example.com/image65.jpg",
            "www.example.com/image66.jpg",
            "www.example.com/image67.jpg",
            "www.example.com/image68.jpg",
            "www.example.com/image69.jpg",
            "www.example.com/image70.jpg",
            "www.example.com/image71.jpg",
            "www.example.com/image72.jpg",
            "www.example.com/image73.jpg",
            "www.example.com/image74.jpg",
            "www.example.com/image75.jpg",
            "www.example.com/image76.jpg",
            "www.example.com/image77.jpg",
            "www.example.com/image78.jpg",
            "www.example.com/image79.jpg",
            "www.example.com/image80.jpg",
            "www.example.com/image81.jpg",
            "www.example.com/image82.jpg",
            "www.example.com/image83.jpg",
            "www.example.com/image84.jpg",
            "www.example.com/image85.jpg",
            "www.example.com/image86.jpg",
            "www.example.com/image87.jpg",
            "www.example.com/image88.jpg",
            "www.example.com/image89.jpg",
            "www.example.com/image90.jpg",
            "www.example.com/image91.jpg",
            "www.example.com/image92.jpg",
            "www.example.com/image93.jpg",
            "www.example.com/image94.jpg",
            "www.example.com/image95.jpg",
            "www.example.com/image96.jpg",
            "www.example.com/image97.jpg",
            "www.example.com/image98.jpg",
            "www.example.com/image99.jpg",
            "www.example.com/image100.jpg"
        };

        public static List<string> CookingSteps = new List<string>
        {
            "Take clean, peeled potatoes and add them to the mixture.",
            "Chop fresh tomatoes into small pieces and set them aside.",
            "Season the chicken breast with salt and pepper.",
            "Heat a pan with olive oil over medium heat.",
            "Add minced garlic and sauté until fragrant.",
            "Dice onions and add them to the garlic in the pan.",
            "Stir-fry the onions until they become translucent.",
            "Cut bell peppers into strips and add them to the pan.",
            "Toss in sliced mushrooms and cook until they release their moisture.",
            "Boil water in a large pot and add a pinch of salt.",
            "Cook pasta until it's al dente, then drain and set aside.",
            "Grill marinated steak to your desired level of doneness.",
            "Whisk eggs in a bowl and season with salt and pepper.",
            "Heat a non-stick skillet and make scrambled eggs.",
            "Rinse rice and cook it with double the amount of water.",
            "Steam broccoli florets until they are tender.",
            "Peel and devein shrimp, then cook them in a hot skillet.",
            "Wash and chop fresh cilantro for garnish.",
            "Preheat the oven to 350°F (175°C).",
            "Grease a baking dish with butter or cooking spray.",
            "Spread a layer of tomato sauce in the baking dish.",
            "Layer lasagna noodles on top of the sauce.",
            "Spread ricotta cheese evenly on the noodles.",
            "Sprinkle shredded mozzarella cheese over the ricotta.",
            "Add cooked ground beef on top of the cheese.",
            "Repeat the layers until all ingredients are used.",
            "Top with a final layer of mozzarella cheese.",
            "Bake the lasagna until it's bubbling and golden brown.",
            "Let it cool for a few minutes before serving.",
            "Slice a baguette into thin rounds.",
            "Toast the baguette slices until they're crispy.",
            "Grate Parmesan cheese for garnish.",
            "Mix flour, salt, and water to make a dough for pizza crust.",
            "Roll out the pizza dough on a floured surface.",
            "Spread pizza sauce evenly over the dough.",
            "Sprinkle shredded cheddar cheese on top of the sauce.",
            "Add your favorite toppings, such as pepperoni and olives.",
            "Bake the pizza in a preheated oven until the crust is golden brown.",
            "Remove the pizza from the oven and let it cool for a few minutes.",
            "Slice the pizza into wedges and serve.",
            "Peel and thinly slice cucumbers for a refreshing salad.",
            "Toss cucumber slices with chopped red onions.",
            "Drizzle olive oil and vinegar over the cucumber salad.",
            "Season the salad with salt, pepper, and dill.",
            "Mix well and refrigerate for a refreshing side dish.",
            "Trim the ends of fresh green beans and blanch them in boiling water.",
            "Sauté minced garlic in butter until fragrant.",
            "Add the blanched green beans to the garlic butter.",
            "Season with salt, pepper, and lemon juice.",
            "Stir-fry until the beans are tender-crisp.",
            "Prepare a marinade with soy sauce, ginger, and garlic for stir-fry.",
            "Slice chicken breast into thin strips and marinate for 30 minutes.",
            "Heat a wok or skillet over high heat with vegetable oil.",
            "Stir-fry marinated chicken until it's cooked through.",
            "Add sliced bell peppers and snap peas to the chicken.",
            "Pour the remaining marinade over the stir-fry.",
            "Toss everything together until the vegetables are crisp-tender.",
            "Garnish with chopped green onions and serve over rice.",
            "Boil eggs until they're hard-boiled, then peel and chop them.",
            "Mix chopped eggs with mayonnaise and mustard.",
            "Season the egg salad with salt, pepper, and paprika.",
            "Spread egg salad on slices of fresh bread for sandwiches.",
            "Slice a ripe avocado and remove the pit.",
            "Mash the avocado in a bowl and season with lime juice and salt.",
            "Spread avocado mash on toasted bread slices.",
            "Top with sliced tomatoes and a sprinkle of red pepper flakes.",
            "Grill cheese sandwiches until they're golden and cheese is melted.",
            "Make a classic BLT sandwich with crispy bacon, lettuce, and tomato slices.",
            "Layer turkey, cranberry sauce, and stuffing on bread for a Thanksgiving sandwich.",
            "Whisk together olive oil, balsamic vinegar, and honey for salad dressing.",
            "Toss mixed greens with the balsamic vinaigrette and add crumbled feta cheese.",
            "Roast a whole chicken with garlic, lemon, and rosemary for a Sunday dinner.",
            "Baste the chicken with its own juices for a crispy skin.",
            "Carve the roasted chicken and serve with pan juices as gravy.",
            "Make a marinara sauce with canned tomatoes, garlic, and basil.",
            "Simmer the sauce until it thickens and season with salt and pepper.",
            "Cook spaghetti until it's al dente and toss with the marinara sauce.",
            "Grate Parmesan cheese over the spaghetti before serving.",
            "Steam asparagus spears until they're tender and bright green.",
            "Drizzle melted butter over the steamed asparagus and sprinkle with Parmesan.",
            "Season grilled corn on the cob with butter, salt, and chili powder.",
            "Serve corn with a squeeze of lime for a zesty flavor.",
            "Prepare a creamy mushroom risotto with Arborio rice and chicken broth.",
            "Sauté sliced mushrooms in butter and shallots until they're browned.",
            "Stir in the Arborio rice and gradually add the chicken broth.",
            "Cook the risotto until it's creamy and the rice is tender.",
            "Finish with grated Parmesan cheese and chopped parsley.",
            "Make a classic beef stew with chunks of beef, carrots, and potatoes.",
            "Brown the beef in a large pot, then add onions and garlic.",
            "Pour in beef broth, diced tomatoes, and season with thyme and bay leaves.",
            "Simmer the stew until the meat is tender and the flavors meld.",
            "Serve hot with crusty bread for dipping.",
            "Marinate salmon fillets in a mixture of soy sauce, ginger, and honey.",
            "Grill the salmon until it's flaky and slightly charred.",
            "Serve with steamed asparagus and lemon wedges.",
            "Bake a batch of chocolate chip cookies with butter, sugar, and eggs.",
            "Mix in chocolate chips and drop spoonfuls of dough onto a baking sheet.",
            "Bake until the cookies are golden brown and gooey in the middle.",
            "Cool on a wire rack before enjoying.",
            "Prepare a fresh fruit salad with a variety of colorful fruits.",
            "Toss fruit pieces with a honey-lime dressing for sweetness.",
            "Garnish with mint leaves for extra flavor.",
            "Boil a pot of water and add tea bags to make hot tea.",
            "Sweeten with sugar or honey and add a slice of lemon.",
            "Serve the hot tea in your favorite cup or mug.",
            "Grill juicy burgers with ground beef, salt, and pepper.",
            "Toast hamburger buns on the grill until they're slightly crispy.",
            "Top the burgers with cheese, lettuce, tomato, and your favorite condiments.",
            "Make a batch of fluffy pancakes with pancake mix and water.",
            "Cook pancakes on a hot griddle until they're golden brown on both sides.",
            "Serve with maple syrup and a pat of butter.",
            "Sear a juicy steak in a hot skillet or on the grill.",
            "Season the steak with salt, pepper, and garlic powder.",
            "Let the steak rest before slicing it thinly.",
            "Make a simple omelette with eggs, salt, and pepper.",
            "Whisk the eggs and pour them into a hot, buttered skillet.",
            "Add your choice of fillings, such as cheese, ham, and bell peppers.",
            "Fold the omelette in half and serve hot.",
            "Grate fresh Parmesan cheese over a bowl of spaghetti carbonara.",
            "Toss cooked spaghetti with a mixture of eggs, cheese, and pancetta.",
            "Season with black pepper and garnish with parsley.",
            "Prepare a delicious caprese salad with sliced tomatoes, fresh mozzarella, and basil leaves.",
            "Drizzle extra virgin olive oil and balsamic vinegar over the salad.",
            "Season with salt and pepper for a burst of flavor.",
            "Roast Brussels sprouts with olive oil, garlic, and bacon.",
            "Bake until the Brussels sprouts are caramelized and tender.",
            "Serve as a savory side dish.",
            "Make a classic tuna salad with canned tuna, mayonnaise, and diced celery.",
            "Season with salt, pepper, and a squeeze of lemon juice.",
            "Spread tuna salad on whole-grain bread for a satisfying sandwich.",
            "Bake a batch of gooey chocolate brownies with melted chocolate and butter.",
            "Mix in cocoa powder, sugar, eggs, and flour for the batter.",
            "Bake until a toothpick comes out with a few moist crumbs.",
            "Cool before cutting into squares.",
            "Grill skewers of marinated chicken and colorful bell peppers.",
            "Thread chicken and peppers onto wooden skewers and grill until charred.",
            "Serve with rice or pita bread and tzatziki sauce.",
            "Prepare a creamy potato soup with diced potatoes, onions, and chicken broth.",
            "Simmer until the potatoes are tender, then blend until smooth.",
            "Stir in heavy cream and garnish with chives.",
            "Grill portobello mushrooms with balsamic glaze for a meaty vegetarian option.",
            "Brush mushrooms with olive oil and balsamic glaze, then grill until tender.",
            "Serve on a bun with your favorite toppings.",
            "Whip up a batch of homemade guacamole with ripe avocados, lime juice, and diced tomatoes.",
            "Season with salt, pepper, and a pinch of cayenne pepper for heat.",
            "Serve with tortilla chips for dipping.",
            "Prepare a classic Caesar salad with crisp romaine lettuce, croutons, and Caesar dressing.",
            "Toss the salad with grated Parmesan cheese and top with grilled chicken strips.",
            "Sauté sliced onions and bell peppers in a hot skillet with olive oil.",
            "Season with salt and pepper and cook until they're caramelized.",
            "Serve as a side dish or on sandwiches.",
            "Make a batch of fluffy buttermilk pancakes with buttermilk, eggs, and flour.",
            "Add a pinch of baking soda and baking powder for extra fluffiness.",
            "Cook until golden brown and serve with maple syrup.",
            "Sear scallops in a hot skillet with butter until they're golden brown on both sides.",
            "Season with salt, pepper, and a squeeze of lemon juice.",
            "Serve hot as an appetizer or over pasta.",
            "Bake a cheesy spinach and artichoke dip with cream cheese, spinach, and artichoke hearts.",
            "Mix in grated Parmesan and mozzarella cheese, then bake until bubbly.",
            "Serve with tortilla chips or toasted bread slices.",
            "Grill corn on the cob with husks on until they're charred and tender.",
            "Peel back the husks and brush with melted butter and seasonings.",
            "Serve hot with lime wedges.",
            "Sauté thinly sliced zucchini and yellow squash with garlic and olive oil.",
            "Season with salt, pepper, and fresh herbs like basil and thyme.",
            "Toss with cooked pasta for a light and tasty dish.",
            "Make a hearty beef chili with ground beef, beans, tomatoes, and chili spices.",
            "Simmer until the flavors meld and serve with shredded cheese and sour cream.",
            "Prepare a classic coleslaw with shredded cabbage and carrots.",
            "Toss with mayonnaise, vinegar, sugar, and celery seeds.",
            "Season with salt and pepper for a refreshing side dish.",
            "Marinate chicken wings in a spicy buffalo sauce for a zesty kick.",
            "Bake the wings until they're crispy and serve with ranch dressing.",
            "Grill marinated shrimp skewers with lemon and herbs for a quick and tasty appetizer.",
            "Thread shrimp onto wooden skewers and grill until pink and charred.",
            "Serve with cocktail sauce for dipping.",
            "Cook a pot of creamy tomato soup with canned tomatoes, onions, and cream.",
            "Simmer until the soup is heated through, then blend until smooth.",
            "Garnish with fresh basil leaves.",
            "Make a batch of fluffy waffles with a waffle iron and waffle batter.",
            "Cook until they're golden and serve with butter and syrup.",
            "Sear a fillet of salmon in a hot skillet with olive oil, skin-side down.",
            "Season with salt and pepper and cook until the skin is crispy.",
            "Flip the salmon and cook until it's flaky and tender.",
            "Serve with a squeeze of lemon.",
            "Prepare a classic chicken noodle soup with chicken broth, diced chicken, and vegetables.",
            "Add cooked noodles to the soup and simmer until heated through.",
            "Season with salt and pepper.",
            "Bake a batch of soft and chewy oatmeal cookies with oats, butter, and brown sugar.",
            "Mix in raisins or chocolate chips for added flavor.",
            "Bake until the cookies are lightly golden.",
            "Make a refreshing cucumber and mint salad with thinly sliced cucumbers and fresh mint leaves.",
            "Toss with a dressing made of yogurt, lemon juice, and garlic.",
            "Season with salt and pepper for a cool and tangy salad.",
            "Sauté sliced mushrooms in butter and garlic until they're browned and tender.",
            "Season with salt, pepper, and fresh thyme for flavor.",
            "Serve as a side dish or on top of grilled steak."
        };


        public static List<string> Utensils = new List<string>
        {
            "Knife",
            "Cutting board",
            "Frying pan",
            "Saucepan",
            "Spatula",
            "Whisk",
            "Mixing bowl",
            "Measuring cups",
            "Measuring spoons",
            "Peeler",
            "Grater",
            "Colander",
            "Tongs",
            "Can opener",
            "Corkscrew",
            "Potato masher",
            "Basting brush",
            "Pastry brush",
            "Rolling pin",
            "Strainer",
            "Ladle",
            "Slotted spoon",
            "Pizza cutter",
            "Garlic press",
            "Salad spinner",
            "Cheese slicer",
            "Pasta fork",
            "Ice cream scoop",
            "Kitchen shears",
            "Mortar and pestle",
            "Timer",
            "Thermometer",
            "Pepper mill",
            "Salt shaker",
            "Bread knife",
            "Chef's knife",
            "Utility knife",
            "Paring knife",
            "Cutting shears",
            "Box grater",
            "Zester",
            "Pastry blender",
            "Whipped cream dispenser",
            "Baster",
            "Meat tenderizer",
            "Bamboo skewers",
            "Egg slicer",
            "Citrus reamer",
            "Nutcracker"
        };

        public static List<string> Ingredients = new List<string>
{
    "Salmon",
    "Tuna",
    "Chicken breast",
    "Ground beef",
    "Pork chops",
    "Shrimp",
    "Lamb",
    "Bacon",
    "Ham",
    "Turkey",
    "Cod",
    "Eggs",
    "Spinach",
    "Broccoli",
    "Carrots",
    "Tomatoes",
    "Onions",
    "Bell peppers",
    "Mushrooms",
    "Potatoes",
    "Sweet potatoes",
    "Rice",
    "Pasta",
    "Bread",
    "Lettuce",
    "Cabbage",
    "Zucchini",
    "Asparagus",
    "Kale",
    "Cauliflower",
    "Celery",
    "Corn",
    "Green beans",
    "Peas",
    "Brussels sprouts",
    "Lentils",
    "Black beans",
    "Chickpeas",
    "Quinoa",
    "Couscous",
    "Tofu",
    "Cheese",
    "Parmesan",
    "Cheddar",
    "Mozzarella",
    "Goat cheese",
    "Feta cheese",
    "Blue cheese",
    "Cream cheese",
    "Sour cream",
    "Greek yogurt",
    "Mayonnaise",
    "Ketchup",
    "Mustard",
    "Soy sauce",
    "Vinegar",
    "Olive oil",
    "Coconut oil",
    "Peanut oil",
    "Sesame oil",
    "Balsamic vinegar",
    "Red wine vinegar",
    "White wine vinegar",
    "Honey",
    "Maple syrup",
    "Brown sugar",
    "Cinnamon",
    "Nutmeg",
    "Garlic",
    "Ginger",
    "Basil",
    "Oregano",
    "Rosemary",
    "Thyme",
    "Parsley",
    "Cilantro",
    "Chives",
    "Dill",
    "Bay leaves",
    "Red pepper flakes",
    "Chili powder",
    "Cumin",
    "Paprika",
    "Coriander",
    "Turmeric",
    "Cardamom",
    "Vanilla extract",
    "Lemon",
    "Lime",
    "Orange",
    "Apples",
    "Bananas",
    "Strawberries",
    "Blueberries",
    "Raspberries",
    "Blackberries",
    "Mango",
    "Pineapple",
    "Avocado",
    "Nuts",
    "Almonds",
    "Walnuts",
    "Pecans",
    "Cashews",
    "Peanuts",
    "Hazelnuts",
    "Pistachios",
    "Sunflower seeds",
    "Pumpkin seeds",
    "Chia seeds",
    "Flaxseeds",
    "Poppy seeds"
};


        public static List<string> Units = new List<string>
        {
            "Teaspoon",
            "Tablespoon",
            "Fluid ounce",
            "Cup",
            "Pint",
            "Quart",
            "Gallon",
            "Milliliter",
            "Liter",
            "Gram",
            "Kilogram",
            "Ounce",
            "Pound",
            "Pinch",
            "Dash",
            "Handful",
            "Dozen",
            "Piece",
            "Slice",
            "Clove",
            "Bunch",
            "Can",
            "Jar",
            "Package",
            "Bag",
            "Box",
            "Bottle",
            "Carton",
            "Packet",
            "Rack"
        };

        public static List<string> CookingAppliances = new List<string>
        {
            "Microwave",
            "Oven",
            "Stove",
            "Refrigerator",
            "Freezer",
            "Toaster",
            "Blender",
            "Food processor",
            "Slow cooker",
            "Pressure cooker",
            "Grill",
            "Griddle",
            "Rice cooker",
            "Coffee maker",
            "Tea kettle",
            "Juicer",
            "Waffle iron",
            "Bread maker",
            "Deep fryer",
            "Toaster oven",
            "Steamer",
            "Mixer",
            "Can opener",
            "Electric skillet",
            "Hand blender",
            "Ice cream maker",
            "Popcorn maker",
            "Sous vide machine",
            "Electric grill pan",
            "Immersion blender"
        };

        public static List<string> Tags = new List<string>
        {
            "Appetizer",
            "Main Course",
            "Dessert",
            "Salad",
            "Soup",
            "Stew",
            "Baking",
            "Grilling",
            "Roasting",
            "Frying",
            "Slow Cooker",
            "Vegan",
            "Vegetarian",
            "Gluten-Free",
            "Dairy-Free",
            "Low-Carb",
            "High-Protein",
            "Keto",
            "Paleo",
            "Mediterranean",
            "Asian",
            "Italian",
            "Mexican",
            "Indian",
            "Greek",
            "French",
            "Chinese",
            "Japanese",
            "Thai",
            "Spanish",
            "Cajun",
            "American",
            "Healthy",
            "Comfort Food",
            "Quick and Easy",
            "One-Pot",
            "Kid-Friendly",
            "Budget-Friendly",
            "Low-Calorie",
            "High-Fiber",
            "Low-Sodium",
            "Spicy",
            "Sweet",
            "Savory",
            "Sour",
            "Salty",
            "Umami",
            "Fresh",
            "Crispy",
            "Creamy",
            "Saucy",
            "Smokey",
            "Tangy",
            "Herb-Flavored",
            "Garlic",
            "Onion",
            "Tomato",
            "Cheese",
            "Pasta",
            "Rice",
            "Potatoes",
            "Chicken",
            "Beef",
            "Pork",
            "Fish",
            "Seafood",
            "Tofu",
            "Egg",
            "Beans",
            "Nuts",
            "Fruit",
            "Chocolate",
            "Caramel",
            "Vanilla",
            "Cinnamon",
            "Spices",
            "Curry",
            "Basil",
            "Cilantro",
            "Rosemary",
            "Thyme",
            "Oregano",
            "Sage",
            "Chili",
            "Paprika",
            "Mustard",
            "Lemon",
            "Lime",
            "Coconut",
            "Mushrooms",
            "Spinach",
            "Kale",
            "Avocado",
            "Berries",
            "Banana",
            "Pineapple",
            "Mango",
            "Strawberries",
            "Blueberries",
            "Raspberries",
            "Blackberries",
            "Apple",
            "Peach",
            "Pear",
            "Orange",
            "Grapes",
            "Cranberries",
            "Cherry"
        };

        public static List<string> IngredientTypes = new List<string>
{
    "Meat",
    "Vegetable",
    "Fruit",
    "Dairy",
    "Grain",
    "Fish",
    "Nut",
    "Seed",
    "Legume",
    "Mushroom",
    "Spice",
    "Herb",
    "Alcohol",
    "Beverage",
    "Sweet",
    "Fat",
    "Sauce",
    "Bread",
    "Pasta",
    "Egg",
    "Seafood",
    "Sugar",
    "Poultry",
    "Red Meat",
    "Game Meat",
    "Root Vegetable",
    "Leafy Vegetable",
    "Tropical Fruit",
    "Berry",
    "Cheese"
};

        public static List<IngredientType> GenerateIngredientTypes()
        {
            var zwrotka = new List<IngredientType>();
            Random random = new Random();
            foreach (string s in IngredientTypes)
            {
                DateTime now = DateTime.Now;
                DateTime createdOn = now.AddDays(-random.Next(365));
              
                zwrotka.Add(new IngredientType
                {
                    Name = s,
                    CreatedOn = createdOn,
                    
                });
            }
            return zwrotka;
        }

        public static List<Author> GenerateRandomAuthors()
        {
            var zwrotka=new List<Author>();
            Random random = new Random();
            foreach (string s in AuthorNames)
            {
                DateTime now = DateTime.Now;
                DateTime createdOn = now.AddDays(-random.Next(365));
                DateTime lastModifiedOn = createdOn.AddHours(random.Next(24));
                zwrotka.Add(new Author
                {
                    AuthorName = s,
                    CreatedOn = createdOn,
                    LastModifiedOn = lastModifiedOn
                });
            }
            return zwrotka;
        }
        public static List<Utensil> GenerateRandomUtensil()
        {
            var zwrotka = new List<Utensil>();
            Random random = new Random();
            foreach (string s in Utensils)
            {
                DateTime now = DateTime.Now;
                DateTime createdOn = now.AddDays(-random.Next(365));
                DateTime lastModifiedOn = createdOn.AddHours(random.Next(24));
                zwrotka.Add(new Utensil
                {
                    Name = s,
                    CreatedOn = createdOn,
                    LastModifiedOn = lastModifiedOn
                });
            }
            return zwrotka;           
        }
        public static List<Tag> GenerateRandomTag()
        {
            var zwrotka = new List<Tag>();
            Random random = new Random();
            foreach (string s in Tags)
            {
                DateTime now = DateTime.Now;
                DateTime createdOn = now.AddDays(-random.Next(365));
              
                zwrotka.Add(new Tag
                {
                    Name = s,
                    CreatedOn = createdOn,                 
                });
            }
            return zwrotka;
        }

        public static List<Photo> GenerateRandomPhoto()
        {
            var zwrotka = new List<Photo>();
            Random random = new Random();
            foreach (string s in AddressList)
            {
                DateTime now = DateTime.Now;
                DateTime createdOn = now.AddDays(-random.Next(365));             
                zwrotka.Add(new Photo
                {
                    Address = s,
                    CreatedOn = createdOn,
                });
            }
            return zwrotka;
        }

        public static List<CookingAppliance> GenerateRandomCookingAppliance()
        {
            var zwrotka = new List<CookingAppliance>();
            Random random = new Random();
            foreach (string s in CookingAppliances)
            {
                DateTime now = DateTime.Now;
                DateTime createdOn = now.AddDays(-random.Next(365));
                zwrotka.Add(new CookingAppliance
                {
                    Name = s,
                    CreatedOn = createdOn,
                });
            }
            return zwrotka;
        }

        public static List<IngredientAmountType> GenerateRandomIngredientAmountType()
        {
            var zwrotka = new List<IngredientAmountType>();
            Random random = new Random();
            foreach (string s in Units)
            {
                DateTime now = DateTime.Now;
                DateTime createdOn = now.AddDays(-random.Next(365));
                zwrotka.Add(new IngredientAmountType
                {
                    UnitName = s,
                    CreatedOn = createdOn,
                });
            }
            return zwrotka;
        }


        public static List<Ingredient> GenerateRandomIngredients(List<IngredientType> ingredientTypes, List<IngredientAmountType> ingredientAmounts)
        {
            var zwrotka = new List<Ingredient>();
            Random random = new Random();
          
            foreach (string s in Ingredients)
            {
                DateTime now = DateTime.Now;
                DateTime createdOn = now.AddDays(-random.Next(365));
                DateTime lastModifiedOn = createdOn.AddHours(random.Next(24));
                zwrotka.Add(new Ingredient
                {
                    IngredientName = s,
                    CreatedOn = createdOn,
                    LastModifiedOn = lastModifiedOn,
                    Type= ingredientTypes.ElementAt(random.Next(ingredientTypes.Count)).IngredientTypeId,
                    IngredientAmountTypeId= ingredientAmounts.ElementAt(random.Next(ingredientAmounts.Count)).IngredientAmountTypeId
                });
            }
            return zwrotka;
        }


        public static async Task Initialize(IServiceProvider serviceProvider)
        {

            using (var scope = serviceProvider.CreateScope())
            {
                var unitOfWork= scope.ServiceProvider.GetRequiredService<IUnitOfWork>();


                var AuthorService = new EntityService<Author>(unitOfWork);              
                await AuthorService.AddRangeAsync(GenerateRandomAuthors());
                var utensilService = new EntityService<Utensil>(unitOfWork);
                await utensilService.AddRangeAsync(GenerateRandomUtensil());
                var tagService = new EntityService<Tag>(unitOfWork);
                await tagService.AddRangeAsync(GenerateRandomTag());                
                var photoService = new EntityService<Photo>(unitOfWork);
                await photoService.AddRangeAsync(GenerateRandomPhoto());                
                var cookingApplianceService = new EntityService<CookingAppliance>(unitOfWork);
                await cookingApplianceService.AddRangeAsync(GenerateRandomCookingAppliance());          
                var ingredientAmountTypeService = new EntityService<IngredientAmountType>(unitOfWork);
                await ingredientAmountTypeService.AddRangeAsync(GenerateRandomIngredientAmountType());
                var ingredientTypeService = new EntityService<IngredientType>(unitOfWork);
                await ingredientTypeService.AddRangeAsync(GenerateIngredientTypes());
                var ingredientService = new EntityService<Ingredient>(unitOfWork);
                await ingredientService.AddRangeAsync(GenerateRandomIngredients((await ingredientTypeService.GetAllAsync()).ToList(),(await ingredientAmountTypeService.GetAllAsync()) .ToList()) );
            }
        }

    }
}
