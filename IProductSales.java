public class ProductSales implements IProduct {
    
    @Override
    public int TotalSales(int[][] productSales) {
        int total = 0;
        for (int i = 0; i < productSales.length; i++) {
            for (int j = 0; j < productSales[i].length; j++) {
                total += productSales[i][j];
            }
        }
        return total;
    }
    
    @Override
    public double AverageSales(int[][] productSales) {
        int total = TotalSales(productSales);
        int count = 0;
        for (int i = 0; i < productSales.length; i++) {
            count += productSales[i].length;
        }
        return (double) total / count;
    }
    
    @Override
    public int MaxSale(int[][] productSales) {
        int max = productSales[0][0];
        for (int i = 0; i < productSales.length; i++) {
            for (int j = 0; j < productSales[i].length; j++) {
                if (productSales[i][j] > max) {
                    max = productSales[i][j];
                }
            }
        }
        return max;
    }
    
    @Override
    public int MinSale(int[][] productSales) {
        int min = productSales[0][0];
        for (int i = 0; i < productSales.length; i++) {
            for (int j = 0; j < productSales[i].length; j++) {
                if (productSales[i][j] < min) {
                    min = productSales[i][j];
                }
            }
        }
        return min;
    }
}