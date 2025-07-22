import { Button } from "@/components/ui/button";
import { useNavigate } from "react-router-dom";

const Header = () => {
  const navigate = useNavigate();

  return (
    <header className="bg-primary py-4 px-6 shadow-md">
      <div className="container mx-auto flex justify-between items-center">
        <div 
          className="text-white text-2xl font-bold cursor-pointer"
          onClick={() => navigate("/")}
        >
          InterviewAI
        </div>
        <nav className="space-x-4">
          <Button 
            variant="ghost" 
            className="text-white hover:text-secondary-light"
            onClick={() => navigate("/setup")}
          >
            Start Interview
          </Button>
        </nav>
      </div>
    </header>
  );
};

export default Header;