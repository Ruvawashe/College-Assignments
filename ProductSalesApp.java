import org.junit.Test;
import static org.junit.Assert.*;

public class ProductSalesTest {
    
    // Test data
    private final int[][] testData = {
        {300, 150, 700},
        {250, 200, 600}
    };
    
    private final ProductSales sales = new ProductSales();
    
    @Test
    public void CalculateTotalSales_ReturnsTotalSales() {
        // Test that TotalSales returns correct total
        int expectedTotal = 2200;
        int actualTotal = sales.TotalSales(testData);
        
        assertEquals("Total sales should be 2200", expectedTotal, actualTotal);
    }
    
    @Test
    public void AverageSales_ReturnsAverageProductSales() {
        // Test that AverageSales returns correct average
        double expectedAverage = 366.67;
        double actualAverage = sales.AverageSales(testData);
        
        assertEquals("Average sales should be approximately 366.67", 
                     expectedAverage, actualAverage, 0.01);
    }
}