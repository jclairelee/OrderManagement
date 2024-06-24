using System;

namespace Assn_One
{
    class Program
    {
        // The Main method serves as the entry point for the console application, which welcomes the user to the restaurant, initializes the menu, and handles user interactions for selecting items, placing orders, and modifying the order.
        // The user can choose to add more items, update quantities, remove items, proceed to checkout, or exit the program. The main loop continues until the user decides to exit.
        static void Main(string[] args)
        {
            // Welcome message
            Console.WriteLine("Welcome to Claire's restaurant!");

            // Initializes the MenuItem[] type of object, which is retrieved from the InitializeMenu() method.
            MenuItem[] menuItems = InitializeMenu();

            // Display all menu items
            DisplayMenu(menuItems);

            // Get the user's initial choice
            int userChoice = GetUserChoice();
            if (userChoice == 9)
            {
                // Exiting the program if the user chooses to exit
                Console.WriteLine("Exiting...");
                return;
            }

            // Retrieve the selected menu item and order quantity
            string selectedMenuItemName = menuItems[userChoice - 1].Name;
            int orderQuantity = GetOrderQty(selectedMenuItemName);

            // Create an OrderItem based on the user's input
            OrderItem order = new OrderItem
            {
                Id = menuItems[userChoice - 1].Id,
                Name = selectedMenuItemName,
                Price = menuItems[userChoice - 1].Price,
                Qty_num = orderQuantity
            };

            // Store a single order object in an array type of OrderItem named "orders"
            OrderItem[] orders = new OrderItem[] { order };

            // Display the initial order details
            ReviewCurrent(orders);

            int optionSelect;
            // Main loop for handling user modifications to the order
            do
            {
                // Get the user's modification option
                optionSelect = GetModificationOption();

                // Handle the selected option
                switch (optionSelect)
                {
                    case 0:
                        // Option to add more items to the order
                        do
                        {
                            DisplayMenu(menuItems);
                            int nextOrderToAdd = GetUserChoice();
                            string itemNameToAdd = menuItems[nextOrderToAdd - 1].Name;
                            int nextOrderQty = GetOrderQty(itemNameToAdd);
                            orders = AddMoreitem(orders, menuItems, orders, nextOrderToAdd, itemNameToAdd, nextOrderQty);

                            Console.WriteLine("Do you want to add more items? (0/N) ");
                        } while (int.TryParse(Console.ReadLine(), out int continueAdding) && (continueAdding == 0));

                        break;

                    case 1:
                        // Option to update the quantity of an item in the order
                        Console.WriteLine($"Updating quantity for {order.Name}");
                        int updatedQty = GetOrderQty(order.Name);
                        order.Qty_num = updatedQty;
                        Console.WriteLine($"Quantity updated to {order.Qty_num} for {order.Name}");

                        break;

                    case 2:
                        // Option to remove an item from the order
                        Console.WriteLine("Enter the index item to remove:");
                        if (int.TryParse(Console.ReadLine(), out int indexToDelete))
                        {
                            orders = DeleteItem(orders, indexToDelete);
                            Console.WriteLine($"{order.Id}");
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a valid index.");
                        }
                        break;

                    case 3:
                        // Option to proceed to checkout
                        ProcessOrder(orders);
                        return;

                    case 4:
                        // Option to exit the program
                        Console.WriteLine("Ending the program as per your selection. Goodbye!");
                        return;

                    default:
                        // Handling invalid options
                        Console.WriteLine("You've entered an invalid option. Please enter a valid option.");
                        Console.WriteLine(" ");
                        break;
                }
            } while (optionSelect != 0);

            // Process the final order after user modifications
            ProcessOrder(orders);
        }

        
        // This static method returns an array of MenuItem objects
        static MenuItem[] InitializeMenu()
            {
                return new MenuItem[]
                {
                    new MenuItem(1, "Fish Taco Fiest Joy", 18.99, 1),
                    new MenuItem(2, "Shrimp Scampi Pasta", 22.99, 1),
                    new MenuItem(3, "Cajun Fried Snapper", 24.99, 1),
                    new MenuItem(4, "Seafood Paella Glee", 28.99, 1),
                    new MenuItem(5, "Oyster Rockefellers", 20.99, 1),
                    new MenuItem(6, "Lobster Bisque Zest", 12.99, 1)
                };
            }

        // DisplayMenu method prints the restaurant menu and prompts the user to select an item.
        // It also provides an option to exit by pressing 9.
        // Parameters:
        // - menuItems: An array of MenuItem objects representing the items in the restaurant menu.
        static void DisplayMenu(MenuItem[] menuItems)
        {
            // Display introductory messages and menu options
            Console.WriteLine(" ");
            Console.WriteLine("Please select an item from the menu by entering its corresponding number.");
            Console.WriteLine("Press 9 to exit.");
            Console.WriteLine(" ");
            Console.WriteLine("------------ M E N U ------------");

            // Loop through each MenuItem and print its details
            for (int i = 0; i < menuItems.Length; i++)
            {
                Console.WriteLine($"{menuItems[i].Id}. {menuItems[i].Name} ${menuItems[i].Price:F2}");
            }

            // Print a separator line at the end of the menu
            Console.WriteLine("---------------------------------");
        }

        // GetModificationOption method prompts the user to select an action for modifying their order.
        // It presents a menu of options, such as adding more items, modifying quantities, removing items,
        // checking out, or exiting the program.
        // Returns:
        // - An integer representing the user's selected modification option.
        static int GetModificationOption()
        {
            // Display modification options
            Console.WriteLine("Select your action: ");
            Console.WriteLine("0. Add more items");
            Console.WriteLine("1. Modify Quantities");
            Console.WriteLine("2. Remove an item");
            Console.WriteLine("3. Checkout");
            Console.WriteLine("4. Exit program");

            // Initialize a variable to store the user's selected option
            int selectedOption;

            // Keep prompting the user until a valid integer is entered
            while (!int.TryParse(Console.ReadLine(), out selectedOption))
            {
                // Display an error message for invalid input
                Console.WriteLine("Invalid input. Please enter a valid option (0, 1, or 2).");
            }

            // Return the valid user-selected modification option
            return selectedOption;
        }



        // GetUserChoice method prompts the user to enter their choice, typically used for selecting
        // an item from the menu. It ensures that the entered input is a valid integer and repeatedly
        // prompts the user until a valid choice is provided.
        // Returns:
        // - An integer representing the user's choice.
        static int GetUserChoice()
        {
            // Display a prompt for the user's choice
            Console.Write("Your choice: ");

            // Initialize a variable to store the user's choice
            int choice;

            // Keep prompting the user until a valid integer is entered
            while (!int.TryParse(Console.ReadLine(), out choice))
            {
                // Display an error message for invalid input
                Console.WriteLine("Invalid input. Please enter an item number.");

                // Prompt the user again for a valid choice
                Console.Write("Enter your choice: ");
            }

            // Return the valid user choice
            return choice;
        }


        // GetOrderQty method prompts the user to enter the quantity for a selected menu item.
        // Parameters:
        // - name: The name of the selected menu item for which the user is entering the quantity.
        // Returns:
        // - An integer representing the quantity entered by the user. If the input is invalid or
        //   not a positive integer, the default quantity of 1 is applied.
        static int GetOrderQty(string name)
        {
            // Display a message indicating the selected menu item
            Console.WriteLine($"You've chosen {name} ");

            // Prompt the user to enter the quantity
            Console.Write($"Enter the quantity: ");

            // Read the user input as a string
            string qtyInput = Console.ReadLine();

            // Default quantity in case of invalid input
            int qtyDefault = 1;

            // Check if the entered input is a valid positive integer
            if (int.TryParse(qtyInput, out int qty) && qty > 0)
            {
                // Display the chosen quantity and item name
                Console.WriteLine($"You've chosen {qty} units of {name}.");
                return qty; // Return the entered quantity
            }
            else
            {
                // Display a message for invalid input and return the default quantity
                Console.WriteLine($"Invalid input detected! The default quantity of 1 will be applied.");
                return qtyDefault;
            }
        }


        // ReviewCurrent method displays a summary of the current order, including the quantity, item name,
        // unit price, and total price for each item in the provided array of OrderItem objects.
        // Parameters:
        // - orders: An array of OrderItem objects representing the customer's current order.
        static void ReviewCurrent(OrderItem[] orders)
        {
            // Display a header for the current order review
            Console.WriteLine("Review your current order: ");

            // Iterate through each item in the orders array and display relevant information
            foreach (var item in orders)
            {
                int counter = 0; // Counter to track the order number
                double totalItemPrice = item.Price * item.Qty_num; // Calculate the total price for the item

                // Display information for each item, including order number, quantity, item name, unit price, and total price
                Console.WriteLine($" {counter + 1}.    {item.Qty_num} X  {item.Name} (${item.Price:F2} each) = ${totalItemPrice:F2}");

                counter += 1; // Increment the counter for the next item
            }
        }


        static OrderItem[] AddMoreitem(OrderItem[] items, MenuItem[] menuItems, OrderItem[] orders, int newItem, string newItemName, int newItemQty )
            {
                Console.WriteLine("Adding more items...");
          
        
                OrderItem nextOrder = new OrderItem
                {
                    Id = menuItems[newItem - 1].Id,
                    Name = newItemName,
                    Price = menuItems[newItem - 1].Price,
                     // Conditional statement to determine the quantity based on a comparison between A and B.
                    Qty_num = newItemQty
                };

                // Create a new array with a larger size
                OrderItem[] updatedOrders = new OrderItem[items.Length + 1];
             
                
                // Copy existing orders to the new array
                for (int i = 0; i < items.Length; i++)
                {
                    updatedOrders[i] = items[i];
                }

                // Add the next order to the new array
                updatedOrders[items.Length] = nextOrder;

                Console.WriteLine("Order Updated.");
                Console.WriteLine($"Total number of orders: {updatedOrders.Length}");

                // Display contents of the updatedOrders array
                Console.WriteLine("Contents of updatedOrders array:");
                for (int i = 0; i < updatedOrders.Length; i++)
                {
                    Console.WriteLine($"Order {i + 1}: Id={updatedOrders[i].Id}, Name={updatedOrders[i].Name}, Price={updatedOrders[i].Price:F2}, Qty={updatedOrders[i].Qty_num}");
                }

                return updatedOrders;
            }

        // DeleteItem method removes an order from the array of OrderItem objects at the specified index.
        // Parameters:
        // - orders: An array of OrderItem objects representing the customer's order.
        // - indexToDelete: The index of the order to be removed from the array.
        // Returns:
        // - An updated array of OrderItem objects with the specified order removed.
        static OrderItem[] DeleteItem(OrderItem[] orders, int indexToDelete)
        {     
            // Check if the specified index is within the valid range
            if (indexToDelete >= 0 && indexToDelete < orders.Length)
            {
                 // Display information about the removed order
                Console.WriteLine($"Order at index {indexToDelete} removed: Id={orders[indexToDelete].Id}, Name={orders[indexToDelete].Name}, Price={orders[indexToDelete].Price:F2}, Qty={orders[indexToDelete].Qty_num}");

                // Create a new array with a smaller size
                OrderItem[] updatedOrders = new OrderItem[orders.Length - 1];

                // Copy items before the deleted index
                for (int i = 0; i < indexToDelete; i++)
                {
                    updatedOrders[i] = orders[i];
                }

                // Copy items after the deleted index
                for (int i = indexToDelete + 1; i < orders.Length; i++)
                {
                    updatedOrders[i - 1] = orders[i];
                }

                return updatedOrders;
            }
            else
            {
                // Display a message for an invalid index, and return the original array unchanged
                Console.WriteLine("Invalid index. No order removed.");
                return orders; 
            }
        }
        // CheckOut method calculates and displays the order summary, including unit prices,
        // quantities, subtotals, HST, and the total cost. It also applies any applicable
        // discount to the total cost and displays the discounted order total if a discount is applied.
        static double CheckOut(OrderItem[] orders, double discountDecimal = 0.00)
        {   
        Console.WriteLine("Order Summary: ");
        Console.WriteLine("--------------------------------------------------------------------");
        Console.WriteLine(" No. |            Item            | Unit Price | Qty |    Total   ");
        Console.WriteLine("--------------------------------------------------------------------");

        
            double subTotal = 0;

            // Display all orders and calculate the total cost
            for(int i=0; i<orders.Length; i++)
            {
                var order = orders[i];
                double eachTotal = order.Price * order.Qty_num;
                Console.WriteLine($"  {i+1}  |     {order.Name}    |     ${order.Price:F2} |  {order.Qty_num}  |   ${eachTotal:F2}");
                subTotal += eachTotal;
            }

            double hstAmount = subTotal * 0.13; // 13% HST
            double totalCosts = subTotal + hstAmount;
            double savings = totalCosts * discountDecimal;
        
            Console.WriteLine($"------------------------------------------------------");
            Console.WriteLine($"                                            Sub Total    ${subTotal:F2}");
            Console.WriteLine($"                                            HST (13%)    ${hstAmount:F2}");
            Console.WriteLine("--------------------------------------------------------------------");
            Console.WriteLine($"                                         Order Total:    ${totalCosts:F2}");  
            if(discountDecimal != 0.00){
            
                Console.WriteLine($" ");
                Console.WriteLine($"************** ${discountDecimal*100}% Discount Applied **************    - ${savings:F2}");
                Console.WriteLine("--------------------------------------------------------------------");
                Console.WriteLine($"                                Discounted Order Total:  ${totalCosts-savings:F2}");
                Console.WriteLine($"  ");

            }

            return totalCosts;
        }
        static (double discountedTotal, double discountPercentage) ApplyDiscountCode(double totalOrderCost)
        {
            Console.Write("Do you have a discount code? (Y/N): ");
            string hasDiscountCodeInput = Console.ReadLine();

            if (hasDiscountCodeInput?.Trim().ToUpper() == "Y")
            {
                Console.Write("Enter the code: ");
                string discountCode = Console.ReadLine();

                double discountPercentage = 0.00;
        
                // Check the discount code and set the discount percentage accordingly
                switch (discountCode?.ToUpper())
                {
                    case "HAPPY2024":
                        discountPercentage = 0.10; // 10% discount
                        break;
                    case "EATMORE":
                        discountPercentage = 0.20; // 20% discount
                        break;
                    
                    default:
                        Console.WriteLine("Invalid discount code. No discount applied.");
                        Console.WriteLine("Discound rate: 0%");
                        
                        return (0.00, discountPercentage); 
                }
                
            
                // Apply the discount
                double discountAmount = totalOrderCost * discountPercentage;
                double discountedTotal = totalOrderCost - discountAmount;

                Console.WriteLine($"******************* {discountPercentage * 100}% Discount Applied *******************");
                Console.WriteLine($"Your Savings: ${discountAmount:F2} ({discountPercentage*100}% OFF)");
                // Console.WriteLine($"Updated Total: ${discountedTotal:F2}");
                // Console.WriteLine($"**********************************************************");

                return (discountedTotal, discountPercentage);
            }
            else
            {
                Console.WriteLine("No discount code entered. Your total remains unchanged.");
                // Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> Your Total: ${totalOrderCost:F2}");
                return (totalOrderCost, 0.00);
            }
            
        }
        // ProcessOrder method calculates the total cost of the customer's order,
        // applies any applicable discounts, and displays the order summary.
        static void ProcessOrder(OrderItem[] orders)
        {
            double totalOrderCost = CheckOut(orders);

            Console.WriteLine(" ");
            Console.WriteLine($"Your total comes to ${totalOrderCost:F2} (13% HST INCLUDED)");

            var (discountedTotal, discountPercentage) = ApplyDiscountCode(totalOrderCost);
            if(discountPercentage != 0.00){   
                Console.WriteLine($"Discounted Total Order Cost: ${discountedTotal:F2}");
                Console.WriteLine($"Discount Percentage: {discountPercentage * 100}%");
                CheckOut(orders, discountPercentage);
            }
            
            Console.WriteLine("Your order has been successfully processed.");
            Console.WriteLine("Thank you for your order!");
        }
        
        // MenuItem class represents an item on the restaurant menu with its properties.
        class MenuItem
        {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Qty_num { get; set; }

        public MenuItem(int id, string name, double price, int qty)
            {
                    Id = id;
                    Name = name;
                    Price = price;
                    Qty_num = qty;
            }
        }
        // OrderItem class represents an item in the customer's order with its properties.
        class OrderItem
        {
            public int Id { get; set; }
            public string? Name { get; set; }
            public double Price { get; set; }
            public int Qty_num { get; set; }
        }
    }
}
