public class ProductSalesReport {
    
    public static void main(String[] args) {
        // Create 2D array with sales data
        int[][] salesData = {
            {300, 150, 700},  // Year 1: Q1, Q2, Q3
            {250, 200, 600}   // Year 2: Q1, Q2, Q3
        };
        
        // Create ProductSales object
        ProductSales sales = new ProductSales();
        
        // Calculate statistics
        int total = sales.TotalSales(salesData);
        double average = sales.AverageSales(salesData);
        int maximum = sales.MaxSale(salesData);
        int minimum = sales.MinSale(salesData);
        
        // Display report
        System.out.println("PRODUCT SALES REPORT - 2025");
        System.out.println("--------------------------------");
        System.out.println("Total sales:    " + total);
        System.out.println("Average sales:  " + Math.round(average));
        System.out.println("Maximum sale:   " + maximum);
        System.out.println("Minimum sale:   " + minimum);
        System.out.println("--------------------------------");
    }
}