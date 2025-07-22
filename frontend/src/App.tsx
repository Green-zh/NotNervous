import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { Toaster } from "@/components/ui/toaster";
import Header from "@/components/Header";
import Index from "@/pages/Index";
import Setup from "@/pages/Setup";
import Interview from "@/pages/Interview";
import Report from "@/pages/Report";
import NotFound from "@/pages/NotFound";
import Preparation from "@/pages/Preparation";

function App() {
  return (
    <Router>
      <div className="min-h-screen">
        <Header />
        <main className="container mx-auto px-4 py-8">
          <Routes>
            <Route path="/" element={<Index />} />
            <Route path="/setup" element={<Setup />} />
            <Route path="/preparation" element={<Preparation />} />
            <Route path="/interview" element={<Interview />} />
            <Route path="/report" element={<Report />} />
            <Route path="*" element={<NotFound />} />
          </Routes>
        </main>
        <Toaster />
      </div>
    </Router>
  );
}

export default App;